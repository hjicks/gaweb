using MASsenger.Core.Entities.UserEntities;
using MASsenger.Core.Enums;

namespace MASsenger.Core.Entities.MessageEntities
{
    public class Message : BaseMessage
    {
        public int SenderId { get; set; }
        public BaseUser Sender { get; set; } = null!;
        public SeenMark SeenMark { get; set; } = SeenMark.Sent;
    }
}
