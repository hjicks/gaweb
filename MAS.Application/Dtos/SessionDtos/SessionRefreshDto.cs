namespace MAS.Application.Dtos.SessionDtos
{
    public record SessionRefreshDto
    {
        public string Jwt { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
