namespace MAS.Application.Dtos.SystemMessageDtos
{
    public record SystemMessageReadDto
    {
        public int DestinationId { set; get; }
        public string? Text { set; get; }
        public DateTime? CreatedAt { set; get; }
    }
}