using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Business.Cqrs;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("PieChart")]
        public async Task<IActionResult> GetPieChartReport()
        {
            var response = await _mediator.Send(new GetPieChartReportQuery());
            return Ok(response);
        }
        [HttpGet("BarChart")]
        public async Task<IActionResult> GetBarChartReport()
        {
            var response = await _mediator.Send(new GetBarChartReportQuery());
            return Ok(response);
        }   [HttpGet("ByStatus")]
        public async Task<IActionResult> GetByStatus()
        {
            var response = await _mediator.Send(new GetStatusReportQuery());
            return Ok(response);
        }
    }
}
