namespace MASsenger.Core.Entities.Base
{
    public class BaseEntity
    {
        public int Id { get; private set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
