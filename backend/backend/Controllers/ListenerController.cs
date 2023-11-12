using backend.DataAccess.Repository;
using backend.Models.API.Listener;
using backend.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListenerController : ControllerBase
    {
        IUnitOfWork _unit;
        static Dictionary<Guid, Thread> _threads = new();
        static Dictionary<Guid, Queue<string>>  _data= new();
        static Dictionary<Guid, bool> _flagger = new();
        public ListenerController(IUnitOfWork unit)
        {
            _unit = unit;
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

            Thread thread = new Thread(ListenerThread);
            thread.Start(request);
            _threads.Add(request.PlotId, thread);
            _flagger.Add(request.PlotId, true);
            _data.Add(request.PlotId, new Queue<string>());

            return Ok("Listener started");
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetAll(Guid id)
        {
            if (!_data.Keys.Contains(id)) return Ok("No data");
            return Ok(_data[id]);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            if (_threads.Keys.Contains(id))
            {
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
                    HttpResponseMessage apiResp = client.GetAsync("").Result;
                    apiResp.EnsureSuccessStatusCode();

                    var json = apiResp.Content.ReadAsStringAsync().Result;

                    rem--;

                    _data[request.PlotId].Enqueue(json);

                    if (_data[request.PlotId].Count > request.Limit) _data[request.PlotId].Dequeue();

                    time = 0;
                }
                Thread.Sleep(1000);
                time++;
            }
        }
    }
}
