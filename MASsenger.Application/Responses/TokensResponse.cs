namespace MASsenger.Application.Responses
{
    public record TokensResponse : BaseResponse
    {
        public string Jwt {  get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public TokensResponse(string jwt)
        {
            Jwt = jwt;
        }
        public TokensResponse(string jwt, Guid refreshToken)
        {
            Jwt = jwt;
            RefreshToken = refreshToken.ToString();
        }
    }
}
