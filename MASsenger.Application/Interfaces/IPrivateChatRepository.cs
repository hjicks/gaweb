using MASsenger.Core.Entities.ChatEntities;

namespace MASsenger.Application.Interfaces
{
    public interface IPrivateChatRepository : IBaseRepository<PrivateChat>
    {
        Task<IEnumerable<PrivateChat>> GetAllAsync();
    }
}
