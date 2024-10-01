using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

 
        [HttpGet("GetMyExpenseForms")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetMyExpenses()
        {
            var query = new GetMyExpensesQuery();
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }


        // GET: api/ExpenseForms/{id}
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetExpenseByIdQuery(id);
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        // GET: api/ExpenseForms/ByManager
        [HttpGet("ByManager")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetByManager()

        {
            var query = new GetExpenseFormsByManager();
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        // GET: api/ExpenseForms/ByAccountant
        [HttpGet("ByAccountant")]
        [Authorize(Roles = "Accountant")]
        public async Task<IActionResult> GetByAccountant()

        {
            var query = new GetExpenseFormsByAccountant();
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        // GET: api/ExpenseForms/ByAccountant
        [HttpGet("ByAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByAdmin()

        {
            var query = new GetExpenseFormsByAdmin();
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        // POST: api/ExpenseForms
        [HttpPost("Create")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Create([FromBody] ExpenseFormRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var command = new CreateExpenseFormCommand(request);
            var response = await _mediator.Send(command);
            return response.IsSuccess ? CreatedAtAction(nameof(GetById), new { id = response.Result.Id }, response) : BadRequest(response);
        }

        // PUT: api/ExpenseForms/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Update(int id, [FromBody] ExpenseFormRequest request)
        {
            var command = new UpdateExpenseFormCommand(id, request);

            var response = await _mediator.Send(command);
            return response.IsSuccess ? NoContent() : BadRequest(response);
        }

        // DELETE: api/ExpenseForms/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteExpenseFormCommand(id);
            var response = await _mediator.Send(command);
            return response.IsSuccess ? NoContent() : BadRequest(response);
        }
        [HttpPut("Reject/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Decline(int id, [FromBody] string rejectionDescription)
        {
            var command = new DeclineExpenseFormCommand(id, rejectionDescription);
            var response = await _mediator.Send(command);
            return response.IsSuccess ? NoContent() : BadRequest(response);
        }

        [HttpPut("Approve/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ApiResponse<object>> Approve(int id)
        {
            var command = new ApproveExpenseFormCommand(id);
            var response = await _mediator.Send(command);
            return response;
        }
        [HttpPut("Pay/{id}")]
        [Authorize(Roles = "Accountant")]
        public async Task<ApiResponse<object>> Pay(int id)
        {
            var command = new PayExpenseFormCommand(id);
            var response = await _mediator.Send(command);
            return response;
        }
        [HttpGet("GetEmployeeExpenseInfo")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetEmployeeExpenseInfo()
        {
            var query = new GetEmployeeExpenseInfoQuery();
            var response = await _mediator.Send(query);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

    }
}
