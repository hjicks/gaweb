using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IBaseChatRepository
    {
        Task<BaseChat?> GetBaseChatByIdAsync(UInt64 chatId);
        Task<IEnumerable<ChannelChat>> GetChannelGroupChatsAsync();
        Task<ChannelChat?> GetChannelGroupChatByIdAsync(UInt64 chatId);
        Task<bool> AddChannelGroupChatAsync(ChannelChat channelGroupChat, User owner);
        Task<bool> Save();
    }
}
