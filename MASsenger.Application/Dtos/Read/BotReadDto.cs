namespace MASsenger.Application.Dtos.Read
{
    public class BotReadDto
    {
        public Int32 Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
        public string Token { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
