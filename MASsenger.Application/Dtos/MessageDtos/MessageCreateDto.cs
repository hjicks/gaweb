namespace MASsenger.Application.Dtos.MessageDtos
{
    public record MessageCreateDto
    {
        public int SenderID { set; get; }
        public int DestinationID { set; get; }
        public string Text { set; get; } = string.Empty;
    }
}
