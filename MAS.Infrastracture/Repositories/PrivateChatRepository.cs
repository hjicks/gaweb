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
        return await _efDbContext.PrivateChats
            .Include(p => p.Members.Where(m => m.Id != userId))
            .Where(p => p.IsDeleted == false)
            .ToListAsync();
    }

    public async Task<PrivateChat?> GetByIdWithMembersAsync(int pvId)
    {
        return await _efDbContext.PrivateChats
            .Where(g => g.Id == pvId)
            .Include(g => g.Members)
            .SingleOrDefaultAsync();
    }
}
