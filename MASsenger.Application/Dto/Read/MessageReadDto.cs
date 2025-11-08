namespace MASsenger.Application.Dto.Read
{
    public class MessageReadDto
    {
        public ulong Id { get; set; }
        public ulong SenderID { get; set; }
        public ulong DestinationID { get; set; }
        public string? Text { get; set; }
        public DateTime? SentTime { get; set;  }
    }
}
