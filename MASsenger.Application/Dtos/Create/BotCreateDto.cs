namespace MASsenger.Application.Dtos.Create
{
    public class BotCreateDto
    {
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Description { get; set; }
        public string Token { get; set; } = null!;
    }
}
