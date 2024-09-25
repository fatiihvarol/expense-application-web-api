using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Business.Cqrs;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ExpenseFormHistoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseFormHistoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        
        [HttpGet("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByExpenseFormId(int Id)
        {
            var query = new GetExpenseFormHistoryQuery(Id);
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

    }
}
