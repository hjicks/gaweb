namespace MASsenger.Application.Dtos.SessionDtos
{
    public record RefreshSessionDto
    {
        public string Jwt { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
