using MASsenger.Core.Entities.Base;

namespace MASsenger.Core.Entities
{
    public class BaseMessage : BaseEntity
    {
        public BaseChat Destination { get; set; } = null!;
        public string Text { get; set; } = string.Empty;
    }
}
