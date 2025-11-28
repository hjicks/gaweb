using MASsenger.Core.Entities.ChatEntities;

namespace MASsenger.Core.Entities.UserEntities
{
    public class User : BaseUser
    {
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;

        // one-to-many relationships
        public ICollection<Bot> BotsOwned { get; set; } = new List<Bot>();
        public ICollection<ChannelChat> ChannelsOwned { get; set; } = new List<ChannelChat>();
        public ICollection<Session> UserSessions { get; set; } = new List<Session>();

        // many-to-many relationships
        public ICollection<Bot> BotsJoined { get; set; } = new List<Bot>();
    }
}
