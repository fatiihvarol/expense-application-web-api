namespace Web.Api.Schema.Reports
{
    public class PieChartReportVM
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public  string? CategoryName { get; set; }
    }
}
