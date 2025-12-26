using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Infrastracture.Database;
using static Dapper.SqlMapper;

namespace MAS.Infrastracture.Repositories.Base;

public class BaseChatRepository : BaseRepository<BaseChat>, IBaseChatRepository
{
    public BaseChatRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
    {
    }

    public async Task<int> GetTypeByIdAsync(int entityId)
    {
        string query = $"SELECT Type FROM Chats WHERE Id == {entityId} AND IsDeleted == false";
        return (await _dapperDbContext.GetConnection().QueryFirstAsync<int>(query));
    }
}
