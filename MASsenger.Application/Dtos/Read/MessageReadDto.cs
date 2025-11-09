namespace MASsenger.Application.Dtos.Read
{
    public class MessageReadDto
    {
        public Int32 SenderID { get; }
        public Int32 DestinationID { get; }
        public string? Text { get; }
        public DateTime? SentTime { get; }
    }
}
