namespace MASsenger.Application.Dtos.ChannelChatDtos
{
    public class ChannelChatReadDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
