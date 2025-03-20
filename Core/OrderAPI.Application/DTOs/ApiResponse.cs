using OrderAPI.Application.Enums;

namespace OrderAPI.Application.DTOs
{
    public class ApiResponse<T>
    {
        public ResponseStatus Status { get; set; }
        public string ResultMessage { get; set; }
        public string ErrorCode { get; set; }
        public T Data { get; set; }
    }
}
