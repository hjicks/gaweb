using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Application.Dtos.PrivateChatDtos
{
    public record PrivateChatReadDto
    {
        public int Id { get; set; }
        public int StarterId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
