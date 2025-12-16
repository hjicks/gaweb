using MAS.Core.Enums;

namespace MAS.Application.Dtos.GroupChatDtos
{
    public record PublicGroupChatUpdateDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string Groupname { get; set; } = string.Empty;
        public string? Description { get; set; }
        public byte[]? Avatar { get; set; }
        public GroupMsgPermissionType MsgPermissionType { get; set; } = GroupMsgPermissionType.AllMembers;
    }
}
