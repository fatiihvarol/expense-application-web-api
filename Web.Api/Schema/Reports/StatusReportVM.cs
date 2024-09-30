namespace Web.Api.Schema.Reports
{
    public class StatusReportVM
    {
        public string Status { get; set; }  // Statü adı
        public Dictionary<string, decimal> AmountsByCurrency { get; set; }

        public string Currency { get; set; }
        public int Count { get; set; }  // Toplam işlem sayısı
    }

}
