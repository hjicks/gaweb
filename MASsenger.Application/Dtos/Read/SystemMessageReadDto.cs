namespace MASsenger.Application.Dtos.Read
{
    public class SystemMessageReadDto
    {
        public Int32 DestinationID { set; get; }
        public string? Text { set; get; }
        public DateTime? CreatedAt { set; get; }
    }
}