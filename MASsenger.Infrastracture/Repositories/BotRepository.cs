using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
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
