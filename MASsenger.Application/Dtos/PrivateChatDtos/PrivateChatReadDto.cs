using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Application.Dtos.PrivateChatDtos
{
    public class PrivateChatReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public BaseUser Receiver { get; set; } = null!;
    }
}
