namespace MASsenger.Application.Dtos.BotDtos
{
    public class BotUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
