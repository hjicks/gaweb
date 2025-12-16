using MAS.Core.Enums;

namespace MAS.Core.Entities.JunctionEntities
{
    public class GroupChatUser
    {
        // the name of these foreign keys helps ef understand that this is a junction
        public int GroupChatId { get; set; }
        public int UserId { get; set; }

        public GroupChatRole Role { get; set; } = GroupChatRole.Member;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        public bool IsBanned { get; set; } = false;
    }
}
