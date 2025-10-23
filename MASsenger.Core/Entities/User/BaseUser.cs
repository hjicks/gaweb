using MASsenger.Core.Enums;

namespace MASsenger.Core.Entities
{
    public class BaseUser
    {
        public UInt64 Id { get; set; }
        public UserType Type { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; } /* acts also as bio */
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; } = false;

        // many-to-many relationships
        public ICollection<ChannelGroupChat> ChannelGroupsJoined = new List<ChannelGroupChat>();
        public ICollection<ChannelGroupChat> ChannelGroupsManaged = new List<ChannelGroupChat>();
        public ICollection<ChannelGroupChat> ChannelGroupsBannedFrom = new List<ChannelGroupChat>();
    }
}
