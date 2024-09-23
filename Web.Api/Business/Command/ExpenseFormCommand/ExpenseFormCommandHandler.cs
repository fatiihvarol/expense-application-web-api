using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Web.Api.Base.Response;
using Web.Api.Data.Entities;
using Web.Api.Schema;
using Web.Api.Data.AppDbContext;
using Web.Api.Business.Cqrs;
using Web.Api.Base.Enums;

namespace Web.Api.Business.Command.ExpenseFormCommand
{
    public class ExpenseFormCommandHandler
        : IRequestHandler<CreateExpenseFormCommand, ApiResponse<ExpenseFormResponse>>,
          IRequestHandler<UpdateExpenseFormCommand, ApiResponse<ExpenseFormResponse>>,
          IRequestHandler<DeleteExpenseFormCommand, ApiResponse<object>>,
          IRequestHandler<DeclineExpenseFormCommand, ApiResponse<object>>,
        IRequestHandler<ApproveExpenseFormCommand, ApiResponse<object>>,
        IRequestHandler<PayExpenseFormCommand, ApiResponse<object>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpenseFormCommandHandler(AppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<ExpenseFormResponse>> Handle(CreateExpenseFormCommand request, CancellationToken cancellationToken)
        {
            // JWT token'dan User Id'yi çekme
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (userId == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("User Id not found in token");
            }

            // User Id ile veritabanında Employee ve Manager bilgilerini sorgulama
            var employee = await _dbContext.VpEmployees.FirstOrDefaultAsync(e => e.Id == Int32.Parse(userId), cancellationToken);
            if (employee == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("Employee not found");
            }

            var manager = await _dbContext.VpManagers.FirstOrDefaultAsync(m => m.Id == employee.ManagerId, cancellationToken);
            if (manager == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("Manager not found");
            }

            if (request.Model.Expenses == null || request.Model.Expenses.Count == 0)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("You can not add empty expense");
            }


            var expenseForm = _mapper.Map<VpExpenseForm>(request);
            expenseForm.EmployeeId = employee.Id;
            expenseForm.ManagerId = manager.Id;
            expenseForm.ExpenseStatusEnum = ExpenseStatusEnum.Pending;
            expenseForm.CreatedDate = DateTime.Now;
            expenseForm.CreateBy = userId;

            if (expenseForm.Expenses != null)
            {
                foreach (var expense in expenseForm.Expenses)
                {
                    expense.CreatedDate = DateTime.Now;
                    expense.CreateBy = userId;
                }
            }

            var fromdb = _dbContext.VpExpenseForms.Add(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }


        public async Task<ApiResponse<ExpenseFormResponse>> Handle(UpdateExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var expenseForm = await _dbContext.VpExpenseForms
                .Include(x => x.Expenses)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken); // Tek bir nesne al

            if (expenseForm == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure("Expense form not found");
            }

            // Modelden expenseForm'a sadece güncellenmesi gereken alanları eşle
            _mapper.Map(request.Model, expenseForm);
            expenseForm.ModifiedDate = DateTime.Now;
            expenseForm.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (expenseForm.Expenses != null)
            {
                foreach (var item in expenseForm.Expenses)
                {
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedBy = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                }
            }
            expenseForm.RejectionDescription = null;
            _dbContext.VpExpenseForms.Update(expenseForm); 
            await _dbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }


        public async Task<ApiResponse<object>> Handle(DeleteExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (userId == null)
            {
                return ApiResponse<object>.Failure("User Id not found in token");
            }

            var expenseForm = await _dbContext.VpExpenseForms
                .Include(x => x.Expenses)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure("Expense form not found");
            }

            expenseForm.IsDeleted = true;
            expenseForm.ModifiedDate = DateTime.Now;
            expenseForm.ModifiedBy = userId;

            if (expenseForm.Expenses != null)
            {
                foreach (var expense in expenseForm.Expenses)
                {
                    expense.IsDeleted = true;
                    expense.ModifiedDate = DateTime.Now;
                    expense.ModifiedBy = userId;
                }
            }


            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<object>.Success("Expense Form Deleted");
        }

        public async Task<ApiResponse<object>> Handle(DeclineExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var expenseForm = await _dbContext.VpExpenseForms.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure("Expense form not found");
            }

            expenseForm.ExpenseStatusEnum = Base.Enums.ExpenseStatusEnum.Rejected;
            expenseForm.RejectionDescription = request.RejectionDescription;
            _dbContext.VpExpenseForms.Update(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<object>.Success("Expense Form Declined");
        }

        public async Task<ApiResponse<object>> Handle(ApproveExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var managerId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            if (managerId == null)
            {
                return ApiResponse<object>.Failure("Manager Id not found in token");
            }

            var expenseForm = await _dbContext.VpExpenseForms.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure("Expense form not found");
            }
            if (expenseForm.ManagerId != Int32.Parse(managerId))
            {
                return ApiResponse<object>.Failure("You are not authorized to approve this expense form");
            }

            expenseForm.ExpenseStatusEnum = Base.Enums.ExpenseStatusEnum.Approved;
            expenseForm.ModifiedBy = managerId;
            expenseForm.ModifiedDate = DateTime.Now;
            _dbContext.VpExpenseForms.Update(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return ApiResponse<object>.Success("Expense Form Approved");
        }

        public Task<ApiResponse<object>> Handle(PayExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var accountantId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (accountantId == null)
            {
                return Task.FromResult(ApiResponse<object>.Failure("Accountant Id not found in token"));
            }

            var expenseForm = _dbContext.VpExpenseForms.FirstOrDefault(e => e.Id == request.Id);
            if (expenseForm == null)
            {
                return Task.FromResult(ApiResponse<object>.Failure("Expense form not found"));
            }

            expenseForm.AccountantId = Int32.Parse(accountantId);
            expenseForm.ExpenseStatusEnum = Base.Enums.ExpenseStatusEnum.Paid;
            expenseForm.ModifiedBy = accountantId;
            expenseForm.ModifiedDate = DateTime.Now;
            _dbContext.VpExpenseForms.Update(expenseForm);
            _dbContext.SaveChanges();

            return Task.FromResult(ApiResponse<object>.Success("Expense Form Paid"));

        }
    }
}
