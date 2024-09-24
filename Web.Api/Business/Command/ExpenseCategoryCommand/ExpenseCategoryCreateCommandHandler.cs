using AutoMapper;
using MediatR;
using Web.Api.Base.Response;
using Web.Api.Business.Cqrs;
using Web.Api.Data.AppDbContext;
using Web.Api.Data.Entities;
using Web.Api.Schema;

namespace Web.Api.Business.Command.ExpenseCategoryCommand
{
    public class ExpenseCategoryCreateCommandHandler :
        IRequestHandler<ExpenseCategoryCreateCommand, ApiResponse<ExpenseCategoryResponse>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public ExpenseCategoryCreateCommandHandler(AppDbContext appDbContext,IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ExpenseCategoryResponse>> Handle(ExpenseCategoryCreateCommand request, CancellationToken cancellationToken)
        {
          
            if (string.IsNullOrEmpty(request.Model.Name))
            {
                return  ApiResponse<ExpenseCategoryResponse>.Failure( "Name is required");
            }
            var category = _mapper.Map<ExpenseCategoryCreateRequest, VpExpenseCategory>(request.Model);

            _appDbContext.VpExpenseCategories.Add(category);
            await _appDbContext.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<VpExpenseCategory, ExpenseCategoryResponse>(category);
            return ApiResponse<ExpenseCategoryResponse>.Success(response);


           
        }
    }
}
