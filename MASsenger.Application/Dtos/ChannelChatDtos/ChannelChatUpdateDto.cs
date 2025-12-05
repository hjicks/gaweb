namespace MASsenger.Application.Dtos.ChannelChatDtos
{
    public record ChannelChatUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
