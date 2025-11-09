using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface ISystemMessageRepository : IBaseRepository<SystemMessage>
    {
        Task<IEnumerable<SystemMessage>> GetAllAsync();
    }
}
