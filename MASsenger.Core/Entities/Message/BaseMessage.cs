namespace MASsenger.Core.Entities
{
    public class BaseMessage
    {
        public Int32 Id { get; set; } // id of the message inside of the chat/user
        public BaseChat Destination { get; set; } = null!;
        public DateTimeOffset SentTime { get; set; } = DateTimeOffset.UtcNow;

        public string Text { get; set; } = string.Empty;
    }
}
