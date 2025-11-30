namespace MASsenger.Application.Responses
{
    public record Result<TResponse> where TResponse : BaseResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Description { get; set; } = string.Empty;
        public TResponse Response { get; set; } = null!;
    }
}
