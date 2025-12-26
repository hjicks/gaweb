using MAS.Core.Enums;

namespace MAS.Application.Dtos.GroupChatDtos;

public record GroupChatUpdateDto
{
    public int Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string? Groupname { get; set; }
    public string? Description { get; set; }
    public string? Avatar { get; set; }
    public GroupMsgPermissionType MsgPermissionType { get; set; } = GroupMsgPermissionType.AllMembers;
}
