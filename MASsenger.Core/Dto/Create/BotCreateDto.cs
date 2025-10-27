namespace MASsenger.Core.Dto.Create
{
    public class BotCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
        public string Token { get; set; } = null!;
    }
}
