using MASsenger.Core.Enums;

namespace MASsenger.Core.Entities
{
    public class BaseUser
    {
        public UInt64 Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; } /* acts also as bio */
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsVerified { get; set; } = false;

        // many-to-many relationships
        public ICollection<ChannelChat> ChannelsJoined = new List<ChannelChat>();
        public ICollection<ChannelChat> ChannelsManaged = new List<ChannelChat>();
        public ICollection<ChannelChat> ChannelsBannedFrom = new List<ChannelChat>();
    }
}
