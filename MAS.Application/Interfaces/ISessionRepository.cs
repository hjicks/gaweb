using MAS.Core.Entities.UserEntities;

namespace MAS.Application.Interfaces;

public interface ISessionRepository : IBaseRepository<Session>
{
    Task<IEnumerable<Session>> GetAllAsync();
    Task<bool> GetActiveAsync(int userId);
    Task<Session?> GetByTokenAsync(Guid sessionToken);
}
