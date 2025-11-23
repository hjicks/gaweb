namespace MASsenger.Core.Entities
{
    public class Session
    {
        public Int32 Id { get; private set; }
        public Guid Token { get; private set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; private set; } = DateTime.UtcNow.AddDays(7);
        public User User { get; set; } = null!;
        public bool IsExpired { get; set; } = false;
    }
}
