using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Application.Dtos.Read
{
    public class PrivateChatReadDto
    {
        public Int32 Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public BaseUser Receiver { get; set; } = null!;
    }
}
