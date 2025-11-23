namespace MASsenger.Application.Dtos.Read
{
    public class MessageReadDto
    {
        public Int32 Id { get; set; }
        public Int32 SenderID { get; set; }
        public Int32 DestinationID { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTimeOffset SentTime { get; set; } = DateTimeOffset.UtcNow;
    }
}
