using MASsenger.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace MASsenger.Core.Dto
{
    public class BotDto
    {
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
        public string Token { get; set; } = null!;
    }
}
