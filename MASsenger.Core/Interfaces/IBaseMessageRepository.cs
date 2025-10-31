using MASsenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Core.Interfaces
{
    public interface IBaseMessageRepository
    {
        Task<IEnumerable<BaseMessage>> GetBaseMessagesAsync();
        Task<bool> AddBaseMessageAsync(BaseMessage baseMessage);
        Task<BaseMessage?> GetBaseMessageByIdAsync(UInt64 msgId);
        Task<bool> UpdateBaseMessageAsync(BaseMessage message);
        Task<bool> DeleteBaseMessageAsync(BaseMessage message);
        Task<bool> Save();
    }
}
