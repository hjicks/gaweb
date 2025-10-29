using MASsenger.Core.Entities;

namespace MASsenger.Core.Interfaces
{
    public interface IBaseUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(UInt64 userId);
        Task<bool> AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<IEnumerable<Bot>> GetAllBotsAsync();
        Task<Bot?> GetBotByIdAsync(UInt64 botId);
        Task<bool> AddBotAsync(Bot bot, User user);
        Task<bool> UpdateBotAsync(Bot bot);
        Task<bool> Save();
    }
}
