using System.Net;

namespace MASsenger.Application.Responses
{
    public record Result<TResponse> where TResponse : BaseResponse
    {
        public bool Success { get; set; }
        public TResponse Response { get; set; } = null!;
        public HttpStatusCode StatusCode { get; set; }
    }
}
