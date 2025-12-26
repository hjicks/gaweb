namespace MAS.Application.Dtos.UserDtos;

public record UserTokenDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? Avatar { get; set; }
    public bool IsVerified { get; set; }
    public bool IsBot { get; set; }
    public string Jwt { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
