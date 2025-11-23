namespace MASsenger.Core.Entities
{
    public class Session
    {
        public Int32 Id { get; private set; }
        public Guid Token { get; private set; } = Guid.NewGuid();
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ExpiresAt { get; private set; } = DateTimeOffset.UtcNow.AddDays(7);
        public User User { get; set; } = null!;
        public bool IsExpired { get; set; } = false;
    }
}
