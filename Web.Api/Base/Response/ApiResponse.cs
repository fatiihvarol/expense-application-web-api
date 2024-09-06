using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Api.Base.Response
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Result { get; set; }
        public string ErrorMessage { get; set; }

        public ApiResponse()
        {
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }

        public ApiResponse(T result)
        {
            IsSuccess = true;
            Result = result;
            ErrorMessage = string.Empty;
        }

        public ApiResponse(Exception ex)
        {
            IsSuccess = false;
            Result = default(T);
            ErrorMessage = ex.Message;
        }
    }
}
