using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Repositories;

public class PrivateChatRepository : BaseRepository<PrivateChat>, IPrivateChatRepository
{
    public PrivateChatRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
    {
        
    }

    public async Task<IEnumerable<PrivateChat>> GetAllAsync()
    {
        string query = "SELECT * FROM Chats WHERE Type == 1";
        return (await _dapperDbContext.GetConnection().QueryAsync<PrivateChat>(query)).ToList();
    }

    public async Task<IEnumerable<PrivateChat>> GetAllUserAsync(int userId)
    {
        var privateChatsIds = await _efDbContext.PrivateChatUsers
            .Where(p => p.UserId == userId)
            .Select(p => p.PrivateChatId)
            .ToListAsync();
        var privateChats = await _efDbContext.PrivateChats
            .Where(p => privateChatsIds.Contains(p.Id) && p.IsDeleted == false)
            .Include(p => p.Members.Where(m => m.Id != userId))
            .ToListAsync();
        return privateChats;
    }

    public async Task<PrivateChat?> GetByIdWithMembersAsync(int pvId)
    {
        return await _efDbContext.PrivateChats
            .Where(g => g.Id == pvId)
            .Include(g => g.Members)
            .SingleOrDefaultAsync();
    }
}
