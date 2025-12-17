using MAS.Core.Entities.UserEntities;

namespace MAS.Application.Dtos.PrivateChatDtos;

public record PrivateChatGetDto
{
    public int Id { get; set; }
    public ICollection<User> Members { get; set; } = new List<User>();
    public DateTime CreatedAt { get; set; }
}
