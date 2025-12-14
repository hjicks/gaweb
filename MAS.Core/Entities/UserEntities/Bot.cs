namespace MAS.Core.Entities.UserEntities
{
    public class Bot : BaseUser
    {
        public string Token { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;
        public bool IsActive { get; set; } = false;

        // many-to-many relationships
        public ICollection<User> Members { get; set; } = new List<User>();
    }
}
