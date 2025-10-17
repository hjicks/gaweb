namespace MASsenger
{
    public class Bot : BaseUser
    {
        public string Token { get; set; } = null!;
        public bool IsActive { get; set; } = false;

        // Required one-to-many relationship with user
        public int OwnerId { get; set; }
        public User Owner { get; set; } = null!;

        // Many-to-many relationships
        public ICollection<User> Members { get; set; } = new List<User>();
        public ICollection<Channel> Channels { get; set; } = new List<Channel>();
        public ICollection<Group> Groups { get; set; } = new List<Group>();
    }
}