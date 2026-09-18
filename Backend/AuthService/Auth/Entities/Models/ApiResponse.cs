namespace Entities.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public static ApiResponse<T> Ok(T data, string? message = null) =>
            new ApiResponse<T> { Success = true, Data = data, Message = message };

        public static ApiResponse<T> Fail(List<string> errors, string message = "An error occured") =>
            new ApiResponse<T> { Success = false, Errors = errors, Message = message };
    }
}
