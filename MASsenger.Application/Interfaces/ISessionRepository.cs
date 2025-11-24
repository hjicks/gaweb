using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session>> GetAllAsync();
        Task<Session> GetActiveSessionByUserIdAsync(Int32 userId);
        Task AddAsync(Session session);
    }
}
