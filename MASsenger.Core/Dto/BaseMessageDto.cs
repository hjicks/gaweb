using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Core.Dto
{
    public class BaseMessageDto
    {
        public UInt64 Id;
        public MessageType Type { get; set; }
        public BaseUser Sender { get; set; } = null!;
        public BaseChat Destination { get; set; } = null!;
        public DateTime SentTime { get; set; }
        public string Text { get; set; } = null!;
    }
}
