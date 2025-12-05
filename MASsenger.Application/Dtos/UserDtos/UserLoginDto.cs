namespace MASsenger.Application.Dtos.UserDtos
{
    public record UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
