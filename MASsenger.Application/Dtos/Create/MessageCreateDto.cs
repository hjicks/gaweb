namespace MASsenger.Application.Dtos.Create
{
    public class MessageCreateDto
    {
        public Int32 SenderID { set; get; }
        public Int32 DestinationID { set; get; }
        public string Text { set; get; } = string.Empty;
        public DateTimeOffset SentTime { set; get; } = DateTimeOffset.UtcNow;
    }
}
