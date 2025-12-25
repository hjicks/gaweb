namespace MAS.Application.Dtos.SessionDtos;

public record SessionGetDto
{
    public int Id { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string OS { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
