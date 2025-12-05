namespace MASsenger.Application.Dtos.BotDtos
{
    public record BotCreateDto
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Description { get; set; }
        public string Token { get; set; } = null!;
    }
}
