using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Infrastracture.Database;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class BaseChatRepository : IBaseChatRepository
    {
        private readonly EfDbContext _context;
        public BaseChatRepository(EfDbContext context)
        {
            _context = context;
        }
        public async Task<BaseChat?> GetBaseChatByIdAsync(UInt64 chatId)
        {
            return await _context.BaseChats.FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public async Task<IEnumerable<ChannelChat>> GetChannelGroupChatsAsync()
        {
            return await _context.ChannelChats.ToListAsync();
        }

        public async Task<ChannelChat?> GetChannelGroupChatByIdAsync(UInt64 chatId)
        {
            return await _context.ChannelChats.FirstOrDefaultAsync(c => c.Id == chatId);
        }

        public Task<bool> AddChannelGroupChatAsync(ChannelChat channelGroupChat, User owner)
        {
            channelGroupChat.Owner = owner;
            _context.ChannelChats.Add(channelGroupChat);
            return Save();
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
