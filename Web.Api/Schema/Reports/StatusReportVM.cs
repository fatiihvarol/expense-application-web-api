namespace Web.Api.Schema.Reports
{
    public class StatusReportVM
    {
        public string? Status { get; set; } 
        public Dictionary<string, decimal>? AmountsByCurrency { get; set; }
        public int Count { get; set; } 
    }

}
