using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using MASsenger.Infrastracture.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<bool> AddBaseMessageAsync(BaseMessage baseMessage, User sender, BaseChat destinationChat)
        {
            baseMessage.Sender = sender;
            baseMessage.Destination = destinationChat;
            _context.BaseMessages.Add(baseMessage);
            return Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
