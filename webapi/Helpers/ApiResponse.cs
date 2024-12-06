using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapi.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse()
        {
            Success = true;
            Data = default(T);
            Errors = null;
        }

        public ApiResponse(T data)
        {
            Success = true;
            Data = data;
            Errors = null;
        }

        public ApiResponse(List<string> errors)
        {
            Success = false;
            Data = default;
            Errors = errors;
        }
    }
}
