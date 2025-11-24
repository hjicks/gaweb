using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface ISessionRepository : IBaseRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllAsync();
    }
}
