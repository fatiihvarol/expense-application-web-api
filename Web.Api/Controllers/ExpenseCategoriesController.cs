using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Business.Cqrs;
using Web.Api.Schema;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpenseCategoriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public ExpenseCategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await mediator.Send(new GetAllExpenseCategoryQuery());
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseCategoryCreateRequest model)
        {
            var response = await mediator.Send(new ExpenseCategoryCreateCommand(model));
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
