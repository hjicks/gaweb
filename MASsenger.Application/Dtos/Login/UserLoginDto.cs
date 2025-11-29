using System.ComponentModel.DataAnnotations;

namespace MASsenger.Application.Dtos.Login
{
    public class UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
