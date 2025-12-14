namespace MAS.Application.Dtos.BotDtos
{
    public record BotLoginDto
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
    }
}
