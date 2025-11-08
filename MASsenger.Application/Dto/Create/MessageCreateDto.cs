namespace MASsenger.Application.Dto.Create
{
    public class MessageCreateDto
    {
        public ulong SenderID { set; get; }
        public ulong DestinationID { set; get; }
        public string? Text { set; get; }
        public DateTime? SentTime { set; get; }
    }
}
