namespace MASsenger.Application.Dto.Read
{
    public class BotReadDto
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
