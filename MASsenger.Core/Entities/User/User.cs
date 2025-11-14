namespace MASsenger.Core.Entities
{
    public class User : BaseUser
    {
        // one-to-many relationships
        public ICollection<Bot> BotsOwned { get; set; } = new List<Bot>();
        public ICollection<ChannelChat> ChannelsOwned = new List<ChannelChat>();

        // many-to-many relationships
        public ICollection<Bot> BotsJoined { get; set; } = new List<Bot>();
        public ICollection<BaseUser> Blocked { get; set; } = new List<BaseUser>();

        // TODO: define a password policy somewhere, this is way too simple
        // My UNIX-infected mind could not comperend better names :-)
        public string Passwd = string.Empty;
    }
}
