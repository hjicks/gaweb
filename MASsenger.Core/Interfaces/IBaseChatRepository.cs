using MASsenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Core.Interfaces
{
    public interface IBaseChatRepository
    {
        Task<BaseChat?> GetBaseChatByIdAsync(UInt64 chatId);
        Task<IEnumerable<ChannelGroupChat>> GetChannelGroupChatsAsync();
        Task<ChannelGroupChat?> GetChannelGroupChatByIdAsync(UInt64 chatId);
        Task<bool> AddChannelGroupChatAsync(ChannelGroupChat channelGroupChat, User owner);
        Task<bool> Save();
    }
}
