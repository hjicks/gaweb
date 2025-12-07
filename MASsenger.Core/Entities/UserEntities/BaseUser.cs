using MASsenger.Core.Entities.Base;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.MessageEntities;

namespace MASsenger.Core.Entities.UserEntities
{
    public class BaseUser : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Description { get; set; } /* acts also as bio */
        public bool IsVerified { get; set; } = false;

        // many-to-many relationships
        public ICollection<PrivateChat> PrivateChats { get; set; } = new List<PrivateChat>();
        public ICollection<ChannelChat> ChannelsJoined { get; set; } = new List<ChannelChat>();
        public ICollection<ChannelChat> ChannelsManaged { get; set; } = new List<ChannelChat>();
        public ICollection<ChannelChat> ChannelsBannedFrom { get; set; } = new List<ChannelChat>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
