namespace MAS.Application.Dtos.SystemMessageDtos
{
    public record SystemMessageCreateDto
    {
        public int DestinationId { set; get; }
        public string Text { set; get; } = string.Empty;
    }
}
