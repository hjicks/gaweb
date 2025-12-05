namespace MASsenger.Application.Dtos.SystemMessageDtos
{
    public record SystemMessageReadDto
    {
        public int DestinationID { set; get; }
        public string? Text { set; get; }
        public DateTime? CreatedAt { set; get; }
    }
}