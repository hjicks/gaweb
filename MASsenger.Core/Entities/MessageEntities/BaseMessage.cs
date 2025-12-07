using MASsenger.Core.Entities.Base;
using MASsenger.Core.Entities.ChatEntities;

namespace MASsenger.Core.Entities.MessageEntities
{
    public class BaseMessage : BaseEntity
    {
        public int DestinationId { get; set; }
        public BaseChat Destination { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
    }
}
