using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Application.Dto
{
    public class BaseMessageDto
    {
        public UInt64 DestinationID { set;  get; }
        public string Text { set;  get; } = null!;
        public DateTime? SentTime { set;  get; } = null;
    }
}
