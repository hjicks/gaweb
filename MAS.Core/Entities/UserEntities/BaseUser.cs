using MAS.Core.Entities.Base;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.MessageEntities;

namespace MAS.Core.Entities.UserEntities
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
