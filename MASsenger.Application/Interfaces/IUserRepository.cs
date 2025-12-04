using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Application.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> IsExistsAsync(Int32 userId);
    }
}
