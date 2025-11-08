namespace MASsenger.Application.Dtos.Read
{
    public class MessageReadDto
    {
        public Int32 SenderID { get; set; }
        public Int32 DestinationID { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime? SentTime { get; set; } = DateTime.UtcNow;
    }
}
