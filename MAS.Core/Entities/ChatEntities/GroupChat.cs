using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;

namespace MAS.Core.Entities.ChatEntities;

public class GroupChat : BaseChat
{
    public bool IsPublic { get; set; } = false;
    public string DisplayName { get; set; } = string.Empty;

    // equivalent to username for a user, must be unique.
    // it should have different structures for public and private groups
    public string Groupname { get; set; } = string.Empty;

    public string? Description { get; set; }
    public byte[]? Avatar { get; set; }
    public GroupMsgPermissionType MsgPermissionType { get; set; } = GroupMsgPermissionType.AllMembers;

    // navigation properties

    // many-to-many
    public ICollection<GroupChatUser> Members { get; set; } = new List<GroupChatUser>();
}
