namespace MASsenger.Application.Dto.Read
{
    public class MessageReadDto
    {
        public ulong SenderID { get; }
        public ulong DestinationID { get; }
        public string? Text { get; }
        public DateTime? SentTime { get; }
    }
}
