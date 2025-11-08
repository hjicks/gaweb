using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> GetAllAsync();
    }
}
