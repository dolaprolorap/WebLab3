using backend.DataAccess.Repository;
using backend.Models.API.Listener;
using backend.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using System.Text;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenerController : ControllerBase
    {
        Mutex _mutex = new Mutex();
        IUnitOfWork _unit;
        IConfiguration _config;
        IServiceScopeFactory _scopeFactory;
        static Dictionary<Guid, Thread> _threads = new();
        static Dictionary<Guid, Queue<PlotEntry>>  _data= new();
        static Dictionary<Guid, bool> _flagger = new();
        public ListenerController(IUnitOfWork unit, IConfiguration config, IServiceScopeFactory scopeFactory)
        {
            _unit = unit;
            _config = config;
            _scopeFactory = scopeFactory;
        }
        ~ListenerController()
        {
            foreach (var pair in _threads)
            {
                _flagger[pair.Key] = false;
                pair.Value.Join();
            }
        }

        [HttpPost("knock")]
        public IActionResult Knock(KnockRequest request)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", request.Key);

            HttpResponseMessage apiResp = client.GetAsync(request.Url).Result;
            apiResp.EnsureSuccessStatusCode();

            var json = apiResp.Content.ReadAsStringAsync().Result;

            return Ok(json);
        }

        [HttpPost]
        [Authorize]
        public IActionResult StartListener(CreateWorkerRequest request)
        {
            if (_threads.Keys.Contains(request.PlotId)) return BadRequest("KILL EXISTING LISTENER!!!");

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            User user = _unit.UserRepo.ReadFirst(u => u.UserName == identity.Name);

            Plot plot = _unit.PlotRepo.ReadFirst(p => p.PlotId == request.PlotId);

            if (plot == null)
                return BadRequest("Plot don't exist");

            if (plot.UserId != user.UserId)
                return Unauthorized("Acess denied");

            if (!(request.Mode == "get" || request.Mode == "post"))
                return BadRequest("Unsupported mode");

            Thread thread = new Thread(ListenerThread);
            thread.Start(request);
            _threads.Add(request.PlotId, thread);
            _flagger.Add(request.PlotId, true);
            _data.Add(request.PlotId, new Queue<PlotEntry>());

            return Ok("Listener started");
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetAll(Guid id)
        {
            if (!_data.Keys.Contains(id)) return Ok("No data");

            DropBatch(id);

            List<PlotEntry> res = _unit.EntryRepo.ReadWhere(e => e.PlotId == id).ToList();
            List<EntryResponse> responses = new();

            foreach (var entry in res)
            {
                responses.Add(new EntryResponse()
                {
                    Data = entry.Data,
                    Date = entry.Date
                });
            }

            return Ok(responses);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            if (_threads.Keys.Contains(id))
            {
                List<PlotEntry> entries = _unit.EntryRepo.ReadWhere(e => e.PlotId == id).ToList();
                _unit.EntryRepo.RemoveRange(entries);
                _unit.Save();

                Thread thread = _threads[id];
                _flagger[id] = false;
                thread.Join();
                _data.Remove(id);
                _flagger.Remove(id);
                _threads.Remove(id);
            }

            return Ok("Deleted");
        }

        [NonAction]
        void ListenerThread(object? data)
        {
            CreateWorkerRequest request = (CreateWorkerRequest)data;

            int rem = request.Count;
            int time = request.Sleep;
            int limit = int.Parse(_config.GetSection("BatchSize").Value);

            HttpClient client = new()
            {
                BaseAddress = new Uri(request.Url),
            };

            client.DefaultRequestHeaders.Add("x-api-key", request.Key);

            while (rem > 0)
            {
                if (!_flagger[request.PlotId]) return;

                if (time == request.Sleep)
                {
                    HttpResponseMessage apiResp;
                    if (request.Mode == "get")
                        apiResp = client.GetAsync("").Result;
                    else if (request.Mode == "post")
                    {
                        StringContent jsonContent = new(request.Body, Encoding.UTF8, "application/json");
                        apiResp = client.PostAsync("", jsonContent).Result;
                    }
                    else return;
                    
                    apiResp.EnsureSuccessStatusCode();

                    var json = apiResp.Content.ReadAsStringAsync().Result;

                    rem--;

                    _data[request.PlotId].Enqueue(new PlotEntry
                        (
                            guid: Guid.NewGuid(),
                            data: json,
                            date: DateTime.Now,
                            plotId: request.PlotId
                        ));

                    if (_data[request.PlotId].Count >= limit) DropBatch(request.PlotId);

                    time = 0;
                }
                Thread.Sleep(1000);
                time++;
            }
        }

        [NonAction]
        void DropBatch(Guid id)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                _mutex.WaitOne();

                var unit = scope.ServiceProvider.GetService<IUnitOfWork>();

                foreach (var entry in _data[id])
                {
                    unit.EntryRepo.Add(entry);
                }
                unit.Save();

                _mutex.ReleaseMutex();

                _data[id].Clear();
            }
        }
    }
}
