using MAS.Core.Enums;

namespace MAS.Application.Dtos.GroupChatDtos;

public class GroupChatMemberGetDto
{
    public int MemberId { get; set; }
    public GroupChatRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public bool IsBanned { get; set; }
}
