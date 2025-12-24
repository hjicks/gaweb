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

    public async Task<IEnumerable<GroupChat>> GetAllUserAsync(int userId)
    {
        var groupChatsIds = await _efDbContext.GroupChatUsers
            .Where(g => g.MemberId == userId && g.IsBanned == false)
            .Select(g => g.GroupChatId)
            .ToListAsync();
        var groupChats = await _efDbContext.GroupChats
            .Where(g => groupChatsIds.Contains(g.Id) && g.IsDeleted == false)
            .ToListAsync();
        return groupChats;
    }

    public async Task<GroupChat?> GetByGroupnameAsync(string groupname)
    {
        return await _efDbContext.GroupChats.SingleOrDefaultAsync(g => g.Groupname == groupname);
    }

    public async Task<GroupChat?> GetByIdWithMemberAsync(int userId, int groupId)
    {
        return await _efDbContext.GroupChats
            .Where(g => g.Id == groupId)
            .Include(g => g.Members.Where(m => m.MemberId == userId))
            .SingleOrDefaultAsync();
    }

    public async Task<GroupChat?> GetByIdWithMembersAsync(int groupId)
    {
        return await _efDbContext.GroupChats
            .Where(g => g.Id == groupId)
            .Include(g => g.Members)
            .SingleOrDefaultAsync();
    }

    public async Task<bool> IsExistsAsync(string groupname)
    {
        return await _efDbContext.GroupChats.AnyAsync(u => u.Groupname == groupname);
    }
}
