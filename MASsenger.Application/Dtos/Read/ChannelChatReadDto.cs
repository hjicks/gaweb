namespace MASsenger.Application.Dtos.Read
{
    public class ChannelChatReadDto
    {
        public Int32 Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
