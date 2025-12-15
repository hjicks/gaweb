namespace MAS.Application.Dtos.MessageDtos
{
    public record MessageAddDto
    {
        public int DestinationId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
