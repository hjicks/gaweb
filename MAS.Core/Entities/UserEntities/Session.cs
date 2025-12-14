using MAS.Core.Entities.Base;

namespace MAS.Core.Entities.UserEntities
{
    public class Session : BaseEntity
    {
        public Guid Token { get; private set; } = Guid.NewGuid();
        public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow.AddDays(7);
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public bool IsExpired { get; set; }
    }
}
