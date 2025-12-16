using System.ComponentModel;

namespace MAS.Application.Dtos.UserDtos
{
    public record UserLoginDto
    {
#if DEBUG
        [DefaultValue("Admin")]
#endif
        public string Username { get; set; } = string.Empty;
#if DEBUG
        [DefaultValue("sysadmin")]
#endif
        public string Password { get; set; } = string.Empty;
    }
}
