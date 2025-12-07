namespace MASsenger.Application.Dtos.MessageDtos
{
    public record MessageCreateDto
    {
        public int DestinationId { set; get; }
        public string Text { set; get; } = string.Empty;
    }
}
