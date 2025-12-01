using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Responses
{
    public record Result<TResponse> where TResponse : BaseResponse
    {
        public bool Success { get; set; } = false;
        public int StatusCode { get; set; } = StatusCodes.Status400BadRequest;
        public string Description { get; set; } = null!;
        public TResponse Response { get; set; } = null!;
    }
}
