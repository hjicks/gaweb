namespace MASsenger.Application.Dtos.MessageDtos
{
    public record MessageReadDto
    {
        public int Id { get; set; }
        public int SenderID { get; set; }
        public int DestinationID { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
