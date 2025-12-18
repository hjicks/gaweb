using MAS.Application.Dtos.UserDtos;

namespace MAS.Application.Dtos.PrivateChatDtos;

public record PrivateChatGetDto
{
    public int Id { get; set; }
    public UserGetDto Receiver { get; set; } = new UserGetDto();
    public DateTime CreatedAt { get; set; }
}
