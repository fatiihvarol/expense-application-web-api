namespace Web.Api.Schema
{
    public class ExpenseCategoryCreateRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

    }
    public class ExpenseCategoryResponse
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
