namespace Web.Api.Base.Message
{
    public static class ErrorMessage
    {
        public interface ExpenseFormErrorMessage
        {
            public static string ExpenseFormNotFound = "Expense form not found";
            public static string UpdateAuthorizationError = "You are not authorized to update this expense form";


        }
        public interface ExpenseErrorMessage
        {
            public static string EmtyExpenseError = "You can not add empty expense";

        }
        public interface ExpenseCategoryErrorMessage
        {
            public static string NameRequiredError = "Name is required";

        }
        public interface TokenErrorMessage
        {
            public static string UserIdNotFound = "User Id not found in token";
            public static string EmptyModelError = "Model can not be empty";
            public static string UserNotFound = "User not found";

        }
        public interface RefreshTokenErrorMessage
        {
            public static string InvalidToken = "Invalid refresh token.";
            public static string EmptyModelError = "Model can not be empty";
            public static string UserNotFound = "User not found";

        }
        public interface CommonErrorMessage
        {
            public static string EmployeeNotFound = "Employee not found";
            public static string ManagerNotFound = "Manager not found";

        }
        public interface CasheErrorMessage
        {
            public static string NoDataFoundError = "No data found for category";
            public static string ManagerNotFound = "Manager not found";

        }
        public interface ValidationErrorMessage
        {
            public static string TotalAmountError = "Total amount must be greater than 0.";
            public static string CurrencyRequieredError = "Currency is required.";
            public static string CurrencyEnumError = "Currency must be either TRY, USD, EUR, PKR, INR.";
            public static string ExpenseAmountError = "Amount must be between 0 and 5000.";
            public static string LocationError = "Location is required.";

        }
    }
}
