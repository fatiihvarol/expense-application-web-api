namespace Web.Api.Base.Message
{
    public static class SuccesMessaage
    {
        public interface ExpenseFormSuccesMessage
        {
            public static string ExpenseFormApproved = "Expense form approved successfully";
            public static string ExpenseFormDeclined = "Expense form declined";
            public static string ExpenseFormDeleted = "Expense form deleted successfully";
            public static string ExpenseFormPaid = "Expense form paid successfully";
        }


    }
}
