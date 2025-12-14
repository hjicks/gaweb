using MAS.Core.Entities.Base;
using MAS.Core.Entities.ChatEntities;

namespace MAS.Core.Entities.MessageEntities
{
    public class BaseMessage : BaseEntity
    {
        public int DestinationId { get; set; }
        public BaseChat Destination { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
    }
}
