namespace MASsenger.Application.Dto.Create
{
    public class MessageCreateDto
    {
        public ulong SenderID { set; get; }
        public ulong DestinationID { set; get; }
        public string Text { set; get; } = string.Empty;
        public DateTime SentTime { set; get; } = DateTime.UtcNow;
    }
}
