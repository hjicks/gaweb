using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Infrastracture.Database;
using static Dapper.SqlMapper;

namespace MASsenger.Infrastracture.Repositories.Base
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
