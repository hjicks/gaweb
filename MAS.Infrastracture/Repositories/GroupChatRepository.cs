using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Repositories;

public class GroupChatRepository : BaseRepository<GroupChat>, IGroupChatRepository
{
    public GroupChatRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
    {

    }

    public async Task<IEnumerable<GroupChat>> GetAllAsync()
    {
        string query = "SELECT * FROM Chats WHERE Type == 2";
        return (await _dapperDbContext.GetConnection().QueryAsync<GroupChat>(query)).ToList();
    }

    public async Task<GroupChat?> IncludedGetByIdAsync(int groupId)
    {
        return await _efDbContext.GroupChats
            .Where(g => g.Id == groupId)
            .Include(g => g.Members)
            .FirstAsync();
    }
}
