using MASsenger.Core.Entities;

namespace MASsenger.Core.Interfaces
{
    public interface IBaseUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(UInt64 userId);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<bool> AddBotAsync(Bot bot, User user);
        Task<bool> Save();
    }
}
