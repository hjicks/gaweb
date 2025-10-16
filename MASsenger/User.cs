namespace MASsenger
{
    public class User : BaseUser
    {
        // Navigation properties
        public ICollection<Channel> Channels { get; set; } = new List<Channel>();
        public ICollection<Group> Groups { get; set; } = new List<Group>();
        public ICollection<Bot> Bots { get; set; } = new List<Bot>();
    }
}
