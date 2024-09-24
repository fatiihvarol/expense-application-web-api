using System.ComponentModel.DataAnnotations;
using Web.Api.Base.Enums;
using Web.Api.Data.Entities;

namespace Web.Api.Schema
{
    public class ExpenseRequest
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public string? ReceiptNumber { get; set; }
    }
    public class ExpenseResponse
    {
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public ExpenseCategoryResponse Category { get; set; }
        public string? ReceiptNumber { get; set; }
        public int ExpenseFormId { get; set; }
    }
}
