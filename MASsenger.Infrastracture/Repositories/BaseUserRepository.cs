using Dapper;
using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using MASsenger.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class BaseUserRepository : IBaseUserRepository
    {
        private readonly MessengerDbContext _context;
        private readonly DapperDbContext _dapperDbContext;
        public BaseUserRepository(MessengerDbContext context, DapperDbContext dapperDbContext)
        {
            _context = context;
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            
            string query = "SELECT * FROM BaseUsers WHERE Type == 'User'";
            return (await _dapperDbContext.GetConnection().QueryAsync<User>(query)).ToList();
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
        public async Task<IEnumerable<Bot>> GetAllBotsAsync()
        {
            string query = "SELECT * FROM BaseUsers WHERE Type == 'Bot'";
            return (await _dapperDbContext.GetConnection().QueryAsync<Bot>(query)).ToList();
        }

        public async Task<Bot?> GetBotByIdAsync(UInt64 botId)
        {
            return await _context.Bots.FirstOrDefaultAsync(b => b.Id == botId);
        }

        public Task<bool> AddBotAsync(Bot bot, User user)
        {
            bot.Owner = user;
            _context.Bots.Add(bot);
            return Save();
        }
        public Task<bool> UpdateBotAsync(Bot bot)
        {
            _context.Bots.Update(bot);
            return Save();
        }

        public Task<bool> DeleteBotAsync(Bot bot)
        {
            _context.Bots.Remove(bot);
            return Save();
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
