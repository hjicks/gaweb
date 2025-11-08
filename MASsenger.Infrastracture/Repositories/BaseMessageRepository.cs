using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class BaseMessageRepository : IBaseMessageRepository
    {
        private readonly MessengerDbContext _context;
        public BaseMessageRepository(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BaseMessage>> GetBaseMessagesAsync()
        {
            return await _context.BaseMessages.ToListAsync();
        }

        public Task<bool> AddBaseMessageAsync(BaseMessage baseMessage)
        {
            Console.WriteLine(baseMessage.Destination.Id);
            _context.BaseMessages.Add(baseMessage);
            return Save();
        }
        public Task<bool> UpdateBaseMessageAsync(BaseMessage message)
        {
            _context.BaseMessages.Update(message);
            return Save();
        }
        public Task<bool> DeleteBaseMessageAsync(BaseMessage message)
        {
            _context.BaseMessages.Remove(message);
            return Save();
        }

        public async Task<BaseMessage?> GetBaseMessageByIdAsync(UInt64 msgId)
        {
            return await _context.BaseMessages.FirstOrDefaultAsync(c => c.Id == msgId);
        }
        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
