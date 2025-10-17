namespace MASsenger
{
    public class User : BaseUser
    {
        // many-to-one relationships
        public ICollection<Channel> ChannelsOwned { get; set; } = new List<Channel>();
        public ICollection<Group> GroupsOwned { get; set; } = new List<Group>();
        public ICollection<Bot> BotsOwned { get; set; } = new List<Bot>();

        // Many-to-many relationships
        public ICollection<Channel> Channels { get; set; } = new List<Channel>();
        public ICollection<Group> Groups { get; set; } = new List<Group>();
        public ICollection<Bot> Bots { get; set; } = new List<Bot>();
    }
}
