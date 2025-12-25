using MAS.Core.Entities.Base;

namespace MAS.Core.Entities.UserEntities;

public class Session : BaseEntity
{
    public byte[] TokenHash { get; set; } = Array.Empty<byte>();
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string OS { get; set; } = string.Empty;
    public bool IsRevoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // navigation properties

    // one-to-many
    public User User { get; set; } = new User();
}
