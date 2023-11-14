using backend.DataAccess.Repository;
using backend.Models.API.Plot;
using backend.Models.DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlotController : ControllerBase
    {
        IUnitOfWork _unit;

        public PlotController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpPost]
        [Authorize]
        // Создает график, втоматически дополняет Id и UserId
        // Данные о пользователе подтягивает из ClaimsIdentity
        public IActionResult CreatePlot(CreatePlotRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = _unit.UserRepo.ReadFirst(u => u.UserName == identity.Name);

            var plot = new Plot(
                guid: Guid.NewGuid(),
                name: request.Name,
                userId: user.UserId
                );

             _unit.PlotRepo.Add(plot);
            _unit.Save();

            var response = new PlotResponse()
            { 
                Id = plot.PlotId,
                Name = plot.PlotName
            };

            return CreatedAtAction(
                actionName: nameof(GetPlot),
                routeValues: new { id = plot.PlotId },
                value: response
                );
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAll() 
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            IEnumerable<Plot> query = _unit.PlotRepo.ReadWhere(p => p.User.UserName == identity.Name).ToList();
            IEnumerable<PlotResponse> response = new List<PlotResponse>();

            foreach (Plot plot in query)
            {
                PlotResponse resp = new PlotResponse()
                {
                    Id = plot.PlotId,
                    Name = plot.PlotName
                };
                response = response.Append(resp);
            }

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        //Получает данные о графике по Id
        public IActionResult GetPlot(Guid id)
        {
            var plot = _unit.PlotRepo.ReadFirst(p => p.PlotId == id);

            var response = new PlotResponse()
            {
                Id = plot.PlotId,
                Name = plot.PlotName
            };

            return Ok(response);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        // Если график с Id существует, то обновляет данные в нем, и ничего не возвращает
        // Иначе создает новый с указанным Id
        public IActionResult UpsertPlot(Guid id, UpdatePlotRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = _unit.UserRepo.ReadFirst(u => u.UserName == identity.Name);

            var plot = _unit.PlotRepo.ReadFirst(p => p.PlotId == id);

            if (plot != null && plot.User.UserName != user.UserName)
                return Unauthorized("Access denied");

            if (plot != null)
            {
                plot.PlotName = request.Name;

                _unit.PlotRepo.Update(plot);
                _unit.Save();

                return NoContent();
            }
            
            plot = new Plot(
                guid: id,
                name: request.Name,
                userId: user.UserId
                );

            var response = new PlotResponse()
            {
                Id = plot.PlotId,
                Name = plot.PlotName
            };

            _unit.PlotRepo.Add(plot);

            _unit.Save();

            return CreatedAtAction(
                actionName: nameof(GetPlot),
                routeValues: new { id = plot.PlotId },
                value: response);
        }

        [Authorize]
        [HttpDelete("{id:guid}")]
        // Удаляет график по Id
        public IActionResult DeletePlot(Guid id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = _unit.UserRepo.ReadFirst(u => u.UserName == identity.Name);
            var plot = _unit.PlotRepo.ReadFirst(p => p.PlotId == id);

            if(plot is null) return NotFound();

            if (plot.UserId != user.UserId)
                return Unauthorized("Access denied");

            _unit.PlotRepo.Remove(plot);
            _unit.Save();

            return Ok("Deleted");
        }
    }
}
