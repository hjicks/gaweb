namespace MAS.Application.Dtos.UserDtos
{
    public record UserTokenDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Jwt { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
