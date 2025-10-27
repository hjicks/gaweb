using System.ComponentModel.DataAnnotations;

namespace MASsenger.Core.Dto
{
    public class UserDto
    {
        [MinLength(1)]
        [MaxLength(64)]
        public string Name { get; set; } = null!;
        [MinLength(5)]
        [MaxLength(32)]
        [RegularExpression("^[a-zA-Z0-9_]+$")]
        public string? Username { get; set; }
        [MaxLength(70)]
        public string? Description { get; set; }
    }
}
