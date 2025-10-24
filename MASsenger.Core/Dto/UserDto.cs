using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Core.Dto
{
    public class UserDto
    {
        public string Name { get; set; } = null!;
        public string? Username { get; set; }
        public string? Description { get; set; }
    }
}
