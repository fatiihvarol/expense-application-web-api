using MediatR;
using Web.Api.Base.Response;
using Web.Api.Schema;

namespace Web.Api.Business.Cqrs
{
    // Commands
    public record CreateExpenseFormCommand(ExpenseFormRequest Model) : IRequest<ApiResponse<ExpenseFormResponse>>;

    public record UpdateExpenseFormCommand(int Id, ExpenseFormRequest Model) : IRequest<ApiResponse<ExpenseFormResponse>>;
    public record ApproveExpenseFormCommand(int Id) : IRequest<ApiResponse<object>>;

    public record DeclineExpenseFormCommand(int Id, string RejectionDescription) : IRequest<ApiResponse<object>>;
    public record PayExpenseFormCommand(int Id) : IRequest<ApiResponse<object>>;

    public record DeleteExpenseFormCommand(int Id) : IRequest<ApiResponse<object>>;

    // Queries
    public record GetMyExpensesQuery() : IRequest<ApiResponse<List<ExpenseFormResponse>>>;
    public record GetExpenseFormsByManager() : IRequest<ApiResponse<List<ExpenseFormResponse>>>;
    public record GetExpenseFormsByAccountant() : IRequest<ApiResponse<List<ExpenseFormResponse>>>;
    public record GetExpenseFormsByAdmin() : IRequest<ApiResponse<List<ExpenseFormResponse>>>;

    public record GetExpenseByIdQuery(int Id) : IRequest<ApiResponse<ExpenseFormResponse>>;

    public record GetEmployeeExpenseInfoQuery () : IRequest<ApiResponse<EmployeeExpenseInfoVM>>;
}
