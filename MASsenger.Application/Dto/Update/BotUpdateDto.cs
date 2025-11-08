namespace MASsenger.Application.Dto.Update
{
    public class BotUpdateDto
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
