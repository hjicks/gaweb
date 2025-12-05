using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Application.Dtos.PrivateChatDtos
{
    public record PrivateChatReadDto
    {
        public int Id { get; set; }
        public BaseUser Starter { get; set; } = null!;
        public BaseUser Receiver { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
