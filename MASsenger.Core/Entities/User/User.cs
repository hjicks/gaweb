namespace MASsenger.Core.Entities
{
    public class User : BaseUser
    {
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        // one-to-many relationships
        public ICollection<Bot> BotsOwned { get; set; } = new List<Bot>();
        public ICollection<ChannelChat> ChannelsOwned = new List<ChannelChat>();
        public ICollection<Session> UserSessions = new List<Session>();

        // many-to-many relationships
        public ICollection<Bot> BotsJoined { get; set; } = new List<Bot>();
        public ICollection<BaseUser> Blocked { get; set; } = new List<BaseUser>();
    }
}
