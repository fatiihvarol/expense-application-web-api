using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Web.Api.Base.Response;
using Web.Api.Business.Cqrs;
using Web.Api.Data.AppDbContext;
using Web.Api.Data.Entities;
using Web.Api.Schema;

namespace Web.Api.Business.Query.ExpenseCategoryQuery
{
    public class ExpenseCategoryQueryHandler :
        IRequestHandler<GetAllExpenseCategoryQuery, ApiResponse<List<ExpenseCategoryResponse>>>
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "ExpenseCategories";

        public ExpenseCategoryQueryHandler(IMapper mapper, AppDbContext appDbContext, IMemoryCache cache)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _cache = cache;
        }

        public Task<ApiResponse<List<ExpenseCategoryResponse>>> Handle(GetAllExpenseCategoryQuery request, CancellationToken cancellationToken)
        {
            // Try to get from cache
            if (!_cache.TryGetValue(CacheKey, out List<VpExpenseCategory> categories))
            {
                categories = _appDbContext.VpExpenseCategories.ToList();

               
                _cache.Set(CacheKey, categories, TimeSpan.FromMinutes(10));
            }

            var response = _mapper.Map<List<VpExpenseCategory>, List<ExpenseCategoryResponse>>(categories);

            return Task.FromResult(ApiResponse<List<ExpenseCategoryResponse>>.Success(response));
        }
    }
}
