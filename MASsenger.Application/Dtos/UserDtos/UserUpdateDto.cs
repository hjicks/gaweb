using System.ComponentModel.DataAnnotations;

namespace MASsenger.Application.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        [MinLength(1)]
        [MaxLength(64)]
        public string Name { get; set; } = null!;
        [MinLength(5)]
        [MaxLength(32)]
        [RegularExpression("^[a-zA-Z0-9_]+$")]
        public string Username { get; set; } = string.Empty;
        [MinLength(5)]
        [MaxLength(32)]
        public string Password { get; set; } = string.Empty;
        [MaxLength(70)]
        public string? Description { get; set; }
    }
}
