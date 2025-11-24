namespace MASsenger.Application.Responses
{
    public record TokensResponse : BaseResponse
    {
        public string Jwt {  get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
