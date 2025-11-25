using MASsenger.Core.Entities.MessageEntities;

namespace MASsenger.Application.Interfaces
{
    public interface ISystemMessageRepository : IBaseRepository<SystemMessage>
    {
        Task<IEnumerable<SystemMessage>> GetAllAsync();
    }
}
