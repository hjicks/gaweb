namespace MASsenger.Application.Dto.Create
{
    public class SystemMessageCreateDto
    {
        public ulong DestinationID { set; get; }
        public string Text { set; get; } = string.Empty;
        public DateTime SentTime { set; get; } = DateTime.UtcNow;
    }
}
