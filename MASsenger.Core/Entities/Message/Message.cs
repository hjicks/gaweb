using MASsenger.Core.Enums;

namespace MASsenger.Core.Entities
{
    public class Message : BaseMessage
    {
        public BaseUser Sender { get; set; } = null!;
        public SeenMark SeenMark { get; set; } = SeenMark.Sent;
    }
}
