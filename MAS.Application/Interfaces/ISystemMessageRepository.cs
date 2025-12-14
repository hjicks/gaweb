using MAS.Core.Entities.MessageEntities;

namespace MAS.Application.Interfaces
{
    public interface ISystemMessageRepository : IBaseRepository<SystemMessage>
    {
        Task<IEnumerable<SystemMessage>> GetAllAsync();
    }
}
