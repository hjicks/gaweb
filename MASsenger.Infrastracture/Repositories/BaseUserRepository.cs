using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using MASsenger.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class BaseUserRepository : IBaseUserRepository
    {
        private readonly MessengerDbContext _context;
        public BaseUserRepository(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BaseUser>> GetBaseUsersAsync()
        {
            return await _context.BaseUsers.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUserByIdAsync(UInt64 userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            return Save();
        }
        public Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return Save();
        }
        public Task<bool> DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            return Save();
        }

        public Task<bool> AddBotAsync(Bot bot, User user)
        {
            bot.Owner = user;
            _context.Bots.Add(bot);
            return Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
