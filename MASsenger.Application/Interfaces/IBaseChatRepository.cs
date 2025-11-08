using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
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
