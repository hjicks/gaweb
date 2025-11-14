using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByUsernameAsync(string username);
    }
}
