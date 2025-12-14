using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.MessageEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;

namespace MAS.Infrastracture.Repositories
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
