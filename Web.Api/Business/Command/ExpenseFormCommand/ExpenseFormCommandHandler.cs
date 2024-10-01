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
using Web.Api.Business.Helper;
using Web.Api.Base.Message;

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
        private readonly ExpenseFormHistoryHelper _expenseFormHistoryHelper;

        public ExpenseFormCommandHandler(AppDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, ExpenseFormHistoryHelper expenseFormHistoryHelper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _expenseFormHistoryHelper = expenseFormHistoryHelper;
        }
        private string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        }
        private void UpdateTimestampsAndUser(dynamic entity, string userId)
        {
            entity.ModifiedDate = DateTime.Now;
            entity.ModifiedBy = userId;
        }
        public async Task<ApiResponse<ExpenseFormResponse>> Handle(CreateExpenseFormCommand request, CancellationToken cancellationToken)
        {
            // JWT token'dan User Id'yi çekme
            var userId = GetUserId();

            if (userId == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var employee = await _dbContext.VpEmployees.FirstOrDefaultAsync(e => e.Id == Int32.Parse(userId), cancellationToken);
            if (employee == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.CommonErrorMessage.EmployeeNotFound);
            }

            var manager = await _dbContext.VpManagers.FirstOrDefaultAsync(m => m.Id == employee.ManagerId, cancellationToken);
            if (manager == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.CommonErrorMessage.ManagerNotFound);
            }

            if (request.Model.Expenses == null || request.Model.Expenses.Count == 0)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.ExpenseErrorMessage.EmtyExpenseError);
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

            await _expenseFormHistoryHelper.AddHistoryLog(fromdb.Entity.Id, userId, HistoryActionEnum.Created, cancellationToken, _dbContext);

            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }


        public async Task<ApiResponse<ExpenseFormResponse>> Handle(UpdateExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var expenseForm = await _dbContext.VpExpenseForms
                .Include(x => x.Expenses!)
                .ThenInclude(y => y.Category)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


            if (expenseForm == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.ExpenseFormErrorMessage.ExpenseFormNotFound);
            }
            var userId = GetUserId();

            if (userId == null)
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }
            if (expenseForm.EmployeeId != Int32.Parse(userId))
            {
                return ApiResponse<ExpenseFormResponse>.Failure(ErrorMessage.ExpenseFormErrorMessage.UpdateAuthorizationError);
            }

            //mapping
            _mapper.Map(request.Model, expenseForm);


            UpdateTimestampsAndUser(expenseForm, userId);

            if (expenseForm.Expenses != null)
            {
                foreach (var item in expenseForm.Expenses)
                {
                    UpdateTimestampsAndUser(item, userId);

                }
            }
            expenseForm.RejectionDescription = null;
            _dbContext.VpExpenseForms.Update(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _expenseFormHistoryHelper.AddHistoryLog(expenseForm.Id, userId, HistoryActionEnum.Updated, cancellationToken, _dbContext);

            var response = _mapper.Map<ExpenseFormResponse>(expenseForm);
            return ApiResponse<ExpenseFormResponse>.Success(response);
        }


        public async Task<ApiResponse<object>> Handle(DeleteExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();

            if (userId == null)
            {
                return ApiResponse<object>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var expenseForm = await _dbContext.VpExpenseForms
                .Include(x => x.Expenses)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure(ErrorMessage.ExpenseFormErrorMessage.ExpenseFormNotFound);
            }

            expenseForm.IsDeleted = true;

            UpdateTimestampsAndUser(expenseForm, userId);

            expenseForm.ExpenseStatusEnum = ExpenseStatusEnum.Deleted;

            if (expenseForm.EmployeeId != Int32.Parse(userId))
            {
                return ApiResponse<object>.Failure(ErrorMessage.ExpenseFormErrorMessage.UpdateAuthorizationError);
            }

            if (expenseForm.Expenses != null)
            {
                foreach (var expense in expenseForm.Expenses)
                {
                    expense.IsDeleted = true;
                    UpdateTimestampsAndUser(expense, userId);
                }
            }


            await _dbContext.SaveChangesAsync(cancellationToken);
            await _expenseFormHistoryHelper.AddHistoryLog(expenseForm.Id, userId, HistoryActionEnum.Deleted, cancellationToken, _dbContext);

            return ApiResponse<object>.Success(SuccesMessaage.ExpenseFormSuccesMessage.ExpenseFormDeleted);
        }

        public async Task<ApiResponse<object>> Handle(DeclineExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                return
                    ApiResponse<object>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }
            var expenseForm = await _dbContext.VpExpenseForms.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure(ErrorMessage.ExpenseFormErrorMessage.ExpenseFormNotFound);
            }

            expenseForm.ExpenseStatusEnum = ExpenseStatusEnum.Rejected;
            expenseForm.RejectionDescription = request.RejectionDescription;
            _dbContext.VpExpenseForms.Update(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _expenseFormHistoryHelper.AddHistoryLog(expenseForm.Id, userId, HistoryActionEnum.Rejected, cancellationToken, _dbContext);

            return ApiResponse<object>.Success(SuccesMessaage.ExpenseFormSuccesMessage.ExpenseFormDeclined);
        }

        public async Task<ApiResponse<object>> Handle(ApproveExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var managerId = GetUserId();

            if (managerId == null)
            {
                return ApiResponse<object>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var expenseForm = await _dbContext.VpExpenseForms.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure(ErrorMessage.ExpenseFormErrorMessage.ExpenseFormNotFound);
            }
            if (expenseForm.ManagerId != Int32.Parse(managerId))
            {
                return ApiResponse<object>.Failure(ErrorMessage.ExpenseFormErrorMessage.UpdateAuthorizationError);
            }

            expenseForm.ExpenseStatusEnum = ExpenseStatusEnum.Approved;

            UpdateTimestampsAndUser(expenseForm, managerId);

            _dbContext.VpExpenseForms.Update(expenseForm);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _expenseFormHistoryHelper.AddHistoryLog(expenseForm.Id, managerId, HistoryActionEnum.Approved, cancellationToken, _dbContext);

            return ApiResponse<object>.Success(SuccesMessaage.ExpenseFormSuccesMessage.ExpenseFormApproved);
        }

        public async Task<ApiResponse<object>> Handle(PayExpenseFormCommand request, CancellationToken cancellationToken)
        {
            var accountantId = GetUserId();
            if (accountantId == null)
            {
                return ApiResponse<object>.Failure(ErrorMessage.TokenErrorMessage.UserIdNotFound);
            }

            var expenseForm = _dbContext.VpExpenseForms.FirstOrDefault(e => e.Id == request.Id);
            if (expenseForm == null)
            {
                return ApiResponse<object>.Failure(ErrorMessage.ExpenseFormErrorMessage.ExpenseFormNotFound);
            }

            expenseForm.AccountantId = Int32.Parse(accountantId);
            expenseForm.ExpenseStatusEnum = ExpenseStatusEnum.Paid;

            UpdateTimestampsAndUser(expenseForm, accountantId);

            _dbContext.VpExpenseForms.Update(expenseForm);
            _dbContext.SaveChanges();

            await _expenseFormHistoryHelper.AddHistoryLog(expenseForm.Id, accountantId, HistoryActionEnum.Paid, cancellationToken, _dbContext);

            return ApiResponse<object>.Success(SuccesMessaage.ExpenseFormSuccesMessage.ExpenseFormPaid);

        }
    }
}
