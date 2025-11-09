using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IPrivateChatRepository : IBaseRepository<PrivateChat>
    {
        Task<IEnumerable<PrivateChat>> GetAllAsync();
    }
}
