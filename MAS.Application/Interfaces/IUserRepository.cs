using MAS.Core.Entities.UserEntities;

namespace MAS.Application.Interfaces;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdWithPrivateChatsAsync(int userId);
    Task<bool> IsExistsAsync(int userId);
    Task<bool> IsExistsAsync(string username);
}
