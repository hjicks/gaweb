using System.ComponentModel.DataAnnotations;

namespace MASsenger.Application.Dtos.Login
{
    public class UserLoginDto
    {
        [MinLength(5)]
        [MaxLength(32)]
        [RegularExpression("^[a-zA-Z0-9_]+$")]
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
