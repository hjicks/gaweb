using System.ComponentModel;

namespace MAS.Application.Dtos.UserDtos;

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
#if DEBUG
    [DefaultValue("Some kinda client")]
#endif
    public string ClientName {  get; set; } = string.Empty;

#if DEBUG
    [DefaultValue("9Front")]
#endif
    public string OS { get; set; } = string.Empty;
}
