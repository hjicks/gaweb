namespace MASsenger.Application.Dtos.Read
{
    public class SystemMessageReadDto
    {
        public ulong DestinationID { set; get; }
        public string? Text { set; get; }
        public DateTime? SentTime { set; get; }
    }
}