namespace MASsenger.Application.Dtos.UserDtos
{
    public class UserReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }
    }
}
