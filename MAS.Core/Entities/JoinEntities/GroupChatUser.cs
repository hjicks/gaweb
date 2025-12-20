using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;

namespace MAS.Core.Entities.JoinEntities;

// explicit join entity
public class GroupChatUser
{
    public int GroupChatId { get; set; }
    public int MemberId { get; set; }
    public GroupChatRole Role { get; set; } = GroupChatRole.Member;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsBanned { get; set; } = false;

    // navigation properties

    // one-to-many
    public GroupChat GroupChat { get; set; } = new GroupChat();
    public User Member { get; set; } = new User();
}
