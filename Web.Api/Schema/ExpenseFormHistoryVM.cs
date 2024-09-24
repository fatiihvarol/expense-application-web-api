﻿using Web.Api.Base.Enums;

namespace Web.Api.Schema
{
    public class ExpenseFormHistoryVM
    {
        public int Id { get; set; }
        public int ExpenseFormId { get; set; }
        public string? Action { get; set; }
        public string? MadeBy { get; set; }
        public DateTime Date { get; set; }
    }
}
