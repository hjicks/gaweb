using MASsenger.Core.Entities.ChatEntities;

namespace MASsenger.Application.Interfaces
{
    public interface IChannelChatRepository : IBaseRepository<ChannelChat>
    {
        Task<IEnumerable<ChannelChat>> GetAllAsync();
    }
}
