using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Infrastracture.Database;
using static Dapper.SqlMapper;

namespace MAS.Infrastracture.Repositories.Base
{
    public class BaseChatRepository : BaseRepository<BaseChat>, IBaseChatRepository
    {
        public BaseChatRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
        {
        }

        public async Task<string> GetTypeByIdAsync(Int32 entityId)
        {
            string query = $"SELECT Type FROM BaseChats WHERE Id == {entityId}";
            return (await _dapperDbContext.GetConnection().QueryFirstAsync<string>(query));
        }
    }
}
