namespace MASsenger
{
    public class User : BaseUser
    {
        // many-to-many relationships
        public ICollection<Bot> BotsJoined { get; set; } = new List<Bot>();
        public ICollection<BaseUser> Blocked { get; set; } = new List<BaseUser>();

        // one-to-many relationship
        public ICollection<Bot> BotsOwned { get; set; } = new List<Bot>();
        public ICollection<ChannelGroupChat> ChannelGroupsOwned = new List<ChannelGroupChat>();
    }
}
