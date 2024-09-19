using MediatR;
using Web.Api.Base.Response;
using Web.Api.Schema;

namespace Web.Api.Business.Cqrs
{
    // Commands
    public record CreateExpenseFormCommand(ExpenseFormRequest Model) : IRequest<ApiResponse<ExpenseFormResponse>>;

    public record UpdateExpenseFormCommand(int Id, ExpenseFormRequest Model) : IRequest<ApiResponse<ExpenseFormResponse>>;

    public record DeclineExpenseFormCommand(int Id, string RejectionDescription) : IRequest<ApiResponse<object>>;

    public record DeleteExpenseFormCommand(int Id) : IRequest<ApiResponse<object>>;

    // Queries
    public record GetAllExpensesQuery() : IRequest<ApiResponse<List<ExpenseFormResponse>>>;
    public record GetMyExpensesQuery() : IRequest<ApiResponse<List<ExpenseFormResponse>>>;

    public record GetExpensesByEmployeeIdQuery(int EmployeeId) : IRequest<ApiResponse<List<ExpenseFormResponse>>>;

    public record GetExpenseByIdQuery(int Id) : IRequest<ApiResponse<ExpenseFormResponse>>;

    public record GetExpensesByParametersQuery(int EmployeeId, string? Status, decimal Amount) : IRequest<ApiResponse<List<ExpenseFormResponse>>>;
}
