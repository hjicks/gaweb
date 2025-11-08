namespace MASsenger.Application.Dto.Create
{
    public class SystemMessageCreateDto
    {
        public ulong DestinationID { set; get; }
        public string? Text { set; get; }
        public DateTime? SentTime { set; get; }
    }
}
