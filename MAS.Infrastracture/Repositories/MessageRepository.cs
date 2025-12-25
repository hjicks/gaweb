using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.MessageEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Repositories;

public class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
    {
        
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        string query = "SELECT * FROM Messages";
        return (await _dapperDbContext.GetConnection().QueryAsync<Message>(query)).ToList();
    }

    public async Task<IEnumerable<Message>> GetAllChatAsync(int chatId)
    {
        return await _efDbContext.Messages
            .Where(m => m.DestinationId == chatId && m.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<Message?> GetChatLastMessageAsync(int chatId)
    {
        return await _efDbContext.Messages
            .Where(m => m.DestinationId == chatId && m.IsDeleted == false)
            .OrderBy(m => m.CreatedAt)
            .LastOrDefaultAsync();
    }

    public async Task<IEnumerable<Message>> GetChatLastMessagesAsync(int chatId, DateTime createdAt, ushort count)
    {
        return await _efDbContext.Messages
            .Where(m => m.DestinationId == chatId && m.CreatedAt > createdAt && m.IsDeleted == false) 
            .OrderBy(m => m.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
