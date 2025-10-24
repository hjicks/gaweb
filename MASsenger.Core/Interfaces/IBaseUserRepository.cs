using MASsenger.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Core.Interfaces
{
    public interface IBaseUserRepository
    {
        Task<IEnumerable<BaseUser>> GetBaseUsersAsync();
        Task<User?> GetUserByIdAsync(UInt64 userId);
        Task<bool> AddUserAsync(User user);
        Task<bool> AddBotAsync(Bot bot, User user);
        Task<bool> Save();
    }
}
