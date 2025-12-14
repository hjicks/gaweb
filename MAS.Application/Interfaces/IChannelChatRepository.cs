using MAS.Core.Entities.ChatEntities;

namespace MAS.Application.Interfaces
{
    public interface IChannelChatRepository : IBaseRepository<ChannelChat>
    {
        Task<IEnumerable<ChannelChat>> GetAllAsync();
    }
}
