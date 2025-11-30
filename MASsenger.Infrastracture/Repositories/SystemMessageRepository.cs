using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
{
    public class SystemMessageRepository : BaseRepository<SystemMessage>, ISystemMessageRepository
    {
        public SystemMessageRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
        {

        }

        public async Task<IEnumerable<SystemMessage>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseMessages WHERE Type == 'SystemMessage'";
            return (await _dapperDbContext.GetConnection().QueryAsync<SystemMessage>(query)).ToList();
        }
    }
}
