using System;

namespace Web.Api.Base.Response
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Result { get; private set; }
        public string? ErrorMessage { get; private set; }
        public DateTime? ExpiresAt { get; set; }

        private ApiResponse(bool isSuccess, T? result, string? errorMessage)
        {
            IsSuccess = isSuccess;
            Result = result;
            ErrorMessage = errorMessage;
        }

        // Static factory methods for creating responses
        public static ApiResponse<T> Success(T result, DateTime? expiresAt = null)
        {
            return new ApiResponse<T>(true, result, null) { ExpiresAt = expiresAt };
        }

        public static ApiResponse<T> Failure(string errorMessage)
        {
            return new ApiResponse<T>(false, default, errorMessage);
        }

        public static ApiResponse<T> Failure(Exception ex)
        {
            return new ApiResponse<T>(false, default, ex.Message);
        }
    }
}
