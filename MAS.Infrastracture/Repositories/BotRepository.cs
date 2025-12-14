using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.UserEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;

namespace MAS.Infrastracture.Repositories
{
    public class BotRepository : BaseRepository<Bot>, IBotRepository
    {
        public BotRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
        {

        }

        public async Task<IEnumerable<Bot>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseUsers WHERE Type == 'Bot'";
            return (await _dapperDbContext.GetConnection().QueryAsync<Bot>(query)).ToList();
        }
    }
}
