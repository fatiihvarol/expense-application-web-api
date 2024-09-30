using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Api.Base.Response;
using Web.Api.Business.Cqrs;
using Web.Api.Data.AppDbContext;
using Web.Api.Schema.Reports;

namespace Web.Api.Business.Query.ReportQuery
{
    public class ReportQueryHandler :
        IRequestHandler<GetPieChartReportQuery, ApiResponse<List<PieChartReportVM>>>,
        IRequestHandler<GetBarChartReportQuery, ApiResponse<List<BarChartReportVM>>>,
        IRequestHandler<GetStatusReportQuery, ApiResponse<List<StatusReportVM>>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReportQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<PieChartReportVM>>> Handle(GetPieChartReportQuery request, CancellationToken cancellationToken)
        {
            // Tüm kategorileri veritabanından alıyoruz
            var categories = await _context.VpExpenseCategories.ToListAsync(cancellationToken);

            // Harcama verileri kategorilere göre gruplanıyor
            var expenseData = await _context.VpExpenses
                .GroupBy(e => e.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    TotalAmount = g.Count()
                })
                .ToListAsync(cancellationToken);

            int count = 0;
            // Tüm kategorilerin olduğu ve harcama yapılan kategorilerin eşleştiği veri listesi
            var pieChartData = categories.Select(category => new PieChartReportVM
            {
                Id = count++,
                CategoryName = category.CategoryName,
                TotalAmount = expenseData.FirstOrDefault(ed => ed.CategoryId == category.CategoryId)?.TotalAmount ?? 0
            }).ToList();

            // Başarıyla liste olarak geri dönüyor
            return ApiResponse<List<PieChartReportVM>>.Success(pieChartData);
        }

        public async Task<ApiResponse<List<BarChartReportVM>>> Handle(GetBarChartReportQuery request, CancellationToken cancellationToken)
        {
            // Fetch categories
            var categories = await _context.VpExpenseCategories.ToListAsync(cancellationToken);

            // Join ExpenseForm with Expenses and group by CategoryId and Currency from ExpenseForm
            var expenseData = await _context.VpExpenseForms
                .Include(ef => ef.Expenses)  // Include related expenses
                .SelectMany(ef => ef.Expenses.Select(e => new
                {
                    ef.CurrencyEnum,  // Currency from the ExpenseForm
                    e.CategoryId,  // Category from the related Expense
                    e.Amount       // Amount from the Expense
                }))
                .GroupBy(e => new { e.CategoryId, e.CurrencyEnum })  // Group by CategoryId and Currency
                .Select(g => new
                {
                    CategoryId = g.Key.CategoryId,
                    Currency = g.Key.CurrencyEnum.ToString(),
                    TotalAmount = g.Sum(e => e.Amount)  // Sum amounts for that currency in the category
                })
                .ToListAsync(cancellationToken);

            // Map the data into the BarChartReportVM format
            var barChartData = categories.Select(category => new BarChartReportVM
            {
                CategoryName = category.CategoryName,
                AmountsByCurrency = expenseData
                    .Where(ed => ed.CategoryId == category.CategoryId)
                    .ToDictionary(
                        ed => ed.Currency,  // Currency code from ExpenseForm
                        ed => ed.TotalAmount  // Total amount for that currency
                    )
            }).ToList();

            return ApiResponse<List<BarChartReportVM>>.Success(barChartData);
        }
        public async Task<ApiResponse<List<StatusReportVM>>> Handle(GetStatusReportQuery request, CancellationToken cancellationToken)
        {

          
                // Harcama formlarını ve ilgili harcamaları dahil ederek sorguyu oluşturuyoruz
                var expenseData = await _context.VpExpenseForms
                    .Include(ef => ef.Expenses)  // Harcamalar ile ExpenseForm'u ilişkilendiriyoruz
                    .SelectMany(ef => ef.Expenses.Select(e => new
                    {
                        ef.ExpenseStatusEnum,  // Statü (ExpenseForm'un durumu)
                        ef.CurrencyEnum,  // Para birimi (ExpenseForm'un para birimi)
                        e.Amount       // Harcama miktarı
                    }))
                    .GroupBy(e => new { e.ExpenseStatusEnum, e.CurrencyEnum })  // Statü ve para birimine göre grupluyoruz
                    .Select(g => new
                    {
                        Status = g.Key.ExpenseStatusEnum.ToString(),  // Statü
                        Currency = g.Key.CurrencyEnum.ToString(),  // Para birimi
                        TotalAmount = g.Sum(e => e.Amount),  // Toplam harcama miktarı
                        Count = g.Count()  // Toplam işlem sayısı
                    })
                    .ToListAsync(cancellationToken);

                // Statüye göre veriyi grupluyoruz ve her statü için para birimine göre harcamaları sözlük yapısına yerleştiriyoruz
                var statusReportData = expenseData
                    .GroupBy(ed => ed.Status)  // Verileri statüye göre grupluyoruz
                    .Select(g => new StatusReportVM
                    {
                        Status = g.Key,  // Statü adı
                        Count = g.Sum(x => x.Count),  // Statüdeki toplam işlem sayısı
                        AmountsByCurrency = g.ToDictionary(
                            x => x.Currency,  // Para birimi
                            x => x.TotalAmount  // Toplam harcama miktarı
                        )
                    })
                    .ToList();

                // Veriyi başarıyla geri döndürüyoruz
                return ApiResponse<List<StatusReportVM>>.Success(statusReportData);
            



        }

    }
}
