using MASsenger.Core.Enums;

namespace MASsenger.Core.Entities
{
    public class BaseMessage
    {
        public UInt64 Id { get; set; } // id of the message inside of the group/chat/bot
        public BaseChat Destination { get; set; } = null!;
        public DateTime SentTime { get; set; } = DateTime.Now;

        public string Text { get; set; } = null!;
    }
}
