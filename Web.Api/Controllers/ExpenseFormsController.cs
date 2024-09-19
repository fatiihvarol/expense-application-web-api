using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Base.Response;
using Web.Api.Business.Cqrs;
using Web.Api.Schema;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseFormsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExpenseFormsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/ExpenseForms
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllExpensesQuery();
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // GET: api/ExpenseForms/ByEmployeeId/{employeeId}
        [HttpGet("ByEmployeeId/{employeeId}")]
        public async Task<IActionResult> GetByEmployeeId(int employeeId)
        {
            var query = new GetExpensesByEmployeeIdQuery(employeeId);
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // GET: api/ExpenseForms/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetExpenseByIdQuery(id);
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/ExpenseForms/ByParameters
        [HttpGet("ByParameters")]
        public async Task<IActionResult> GetByParameters(
            [FromQuery] int employeeId,
            [FromQuery] string? status,
            [FromQuery] decimal amount)
        {
            var query = new GetExpensesByParametersQuery( employeeId, status, amount);
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // POST: api/ExpenseForms
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExpenseFormRequest request)
        {
            var command = new CreateExpenseFormCommand(request);
            var response = await _mediator.Send(command);
            return response.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = response.Result.Id }, response) : BadRequest(response);
        }

        // PUT: api/ExpenseForms/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseFormRequest request)
        {
          var command = new UpdateExpenseFormCommand(id, request);

            var response = await _mediator.Send(command);
            return response.IsSuccess ? NoContent() : BadRequest(response);
        }

        // DELETE: api/ExpenseForms/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteExpenseFormCommand(id);
            var response = await _mediator.Send(command);
            return response.IsSuccess ? NoContent() : BadRequest(response);
        }
    }
}
