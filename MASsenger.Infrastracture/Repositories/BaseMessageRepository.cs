using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Entities.Message;
using MASsenger.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class BaseMessageRepository : IBaseMessageRepository
    {
        private readonly MessengerDbContext _context;
        private readonly DapperDbContext _dapperDbContext;
        public BaseMessageRepository(MessengerDbContext context, DapperDbContext dapperDbContext)
        {
            _context = context;
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {

            string query = "SELECT * FROM BaseMessages WHERE Type == 'Message'";
            return (await _dapperDbContext.GetConnection().QueryAsync<Message>(query)).ToList();
        }

        public async Task<Message?> GetMessageByIdAsync(UInt64 Id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public Task<bool> AddMessageAsync(Message msg)
        {
            _context.Messages.Add(msg);
            return Save();
        }

        public Task<bool> UpdateMessageAsync(Message msg)
        {
            _context.Messages.Update(msg);
            return Save();
        }

        public Task<bool> DeleteMessageAsync(Message msg)
        {
            _context.Messages.Remove(msg);
            return Save();
        }
        public async Task<IEnumerable<SystemMessage>> GetAllSystemMessagesAsync()
        {
            string query = "SELECT * FROM BaseMessages WHERE Type == 'SystemMessages'";
            return (await _dapperDbContext.GetConnection().QueryAsync<SystemMessage>(query)).ToList();
        }

        public async Task<SystemMessage?> GetSystemMessageByIdAsync(UInt64 Id)
        {
            return await _context.SystemMessages.FirstOrDefaultAsync(sm => sm.Id == Id);
        }

        public Task<bool> AddSystemMessageAsync(SystemMessage sm)
        {
            _context.SystemMessages.Add(sm);
            return Save();
        }
        public Task<bool> UpdateSystemMessageAsync(SystemMessage sm)
        {
            _context.SystemMessages.Update(sm);
            return Save();
        }

        public Task<bool> DeleteSystemMessageAsync(SystemMessage sm)
        {
            _context.SystemMessages.Remove(sm);
            return Save();
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
