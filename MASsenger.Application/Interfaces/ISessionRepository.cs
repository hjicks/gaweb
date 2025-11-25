using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Application.Interfaces
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllAsync();
    }
}
