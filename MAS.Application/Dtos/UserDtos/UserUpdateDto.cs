namespace MAS.Application.Dtos.UserDtos
{
    public record UserUpdateDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Avatar { get; set; }
    }
}
