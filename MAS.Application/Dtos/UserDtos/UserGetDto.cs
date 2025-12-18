namespace MAS.Application.Dtos.UserDtos;

public record UserGetDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public byte[]? Avatar { get; set; }
    public bool IsVerified { get; set; }
    public bool IsBot { get; set; }
    public DateTime LastSeenAt { get; set; }
}
