namespace MASsenger.Core.Entities
{
    public class BaseMessage
    {
        public Int32 Id { get; set; } // id of the message inside of the chat/user
        public BaseChat Destination { get; set; } = null!;
        public DateTime SentTime { get; set; } = DateTime.Now;

        public string Text { get; set; } = null!;
    }
}
