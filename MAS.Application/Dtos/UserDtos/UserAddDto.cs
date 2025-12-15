namespace MAS.Application.Dtos.UserDtos
{
    public record UserAddDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public byte[]? Avatar { get; set; }
    }
}
