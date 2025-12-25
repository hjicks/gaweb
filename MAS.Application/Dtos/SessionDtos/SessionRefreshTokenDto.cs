namespace MAS.Application.Dtos.SessionDtos;

public record SessionRefreshTokenDto
{
    public string RefreshToken { get; set; } = string.Empty;
}
