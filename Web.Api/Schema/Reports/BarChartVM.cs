namespace Web.Api.Schema.Reports
{
    public class BarChartReportVM
    {
        public string? CategoryName { get; set; }
        public Dictionary<string, decimal>? AmountsByCurrency { get; set; } 
    }

}
