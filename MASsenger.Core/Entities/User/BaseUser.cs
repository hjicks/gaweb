using MASsenger.Core.Entities.Base;

namespace MASsenger.Core.Entities
{
    public class BaseUser : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Description { get; set; } /* acts also as bio */
        public bool IsVerified { get; set; } = false;

        // many-to-many relationships
        public ICollection<ChannelChat> ChannelsJoined { get; set; } = new List<ChannelChat>();
        public ICollection<ChannelChat> ChannelsManaged { get; set; } = new List<ChannelChat>();
        public ICollection<ChannelChat> ChannelsBannedFrom { get; set; } = new List<ChannelChat>();
    }
}
