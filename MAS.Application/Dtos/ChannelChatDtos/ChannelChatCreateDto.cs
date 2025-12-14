namespace MAS.Application.Dtos.ChannelChatDtos
{
    public record ChannelChatCreateDto
    {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
