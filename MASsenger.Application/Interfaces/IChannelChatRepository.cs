using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IChannelChatRepository : IBaseRepository<ChannelChat>
    {
        Task<IEnumerable<ChannelChat>> GetAllAsync();
    }
}
