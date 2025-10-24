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
    public class BaseChatRepository : IBaseChatRepository
    {
        private readonly MessengerDbContext _context;
        public BaseChatRepository(MessengerDbContext context)
        {
            _context = context;
        }
        public async Task<BaseChat?> GetBaseChatByIdAsync(UInt64 chatId)
        {
            return await _context.BaseChats.FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<IEnumerable<ChannelGroupChat>> GetChannelGroupChatsAsync()
        {
            return await _context.ChannelGroupChats.ToListAsync();
        }

        public async Task<ChannelGroupChat?> GetChannelGroupChatByIdAsync(UInt64 chatId)
        {
            return await _context.ChannelGroupChats.FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public Task<bool> AddChannelGroupChatAsync(ChannelGroupChat channelGroupChat, User owner)
        {
            channelGroupChat.Owner = owner;
            _context.ChannelGroupChats.Add(channelGroupChat);
            return Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
