using MAS.Core.Entities.Base;

namespace MAS.Core.Entities.UserEntities
{
    public class Session : BaseEntity
    {
        public Guid Token { get; set; } = Guid.NewGuid();
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
        public int UserId { get; set; }
        public string Device { get; set; } = string.Empty;
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // navigation properties

        // one-to-many
        public User User { get; set; } = new User();
    }
}
