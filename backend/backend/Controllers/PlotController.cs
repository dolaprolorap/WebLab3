using backend.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlotController : ControllerBase
    {
        [HttpPost()]
        public IActionResult CreatePlot(CreatePlotRequest request)
        {
            return Ok(request);
        }

        [HttpGet("{id : guid}")]
        public IActionResult GetPlot(Guid id)
        {
            return Ok(id);
        }

        [HttpPut("{id : guid}")]
        public IActionResult Plot(Guid id, UpdatePlotRequest request)
        {
            return Ok(request);
        }

        [HttpDelete("{id : guid}")]
        public IActionResult DeletePlot(Guid id)
        {
            return Ok(id);
        }


    }
}
