using System;
using System.Collections.Generic;
using System.Text;

namespace Dobi.Contracts.Common
{
    public sealed record ApiResponse<T>(
        bool Success,
        string Message,
        T? Data)
    {
        public static ApiResponse<T> Ok(T data, string message = "Request completed successfully.")
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>(false, message, default);
        }
    }
}
