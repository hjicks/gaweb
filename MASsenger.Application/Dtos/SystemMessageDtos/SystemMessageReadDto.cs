namespace MASsenger.Application.Dtos.SystemMessageDtos
{
    public class SystemMessageReadDto
    {
        public int DestinationID { set; get; }
        public string? Text { set; get; }
        public DateTime? CreatedAt { set; get; }
    }
}