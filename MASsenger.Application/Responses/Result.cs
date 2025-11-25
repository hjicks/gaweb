using System.Net;

namespace MASsenger.Application.Responses
{
    public record Result<TResponse> where TResponse : BaseResponse
    {
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Description { get; set; } = string.Empty;
        public TResponse Response { get; set; } = null!;
    }
}
