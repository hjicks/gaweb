using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
{
    public class BotRepository : BaseRepository<Bot>, IBotRepository
    {
        private readonly DapperDbContext _dapperDbContext;
        public BotRepository(EfDbContext efContext, DapperDbContext dapperDbContext) : base(efContext)
        {
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<Bot>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseUsers WHERE Type == 'Bot'";
            return (await _dapperDbContext.GetConnection().QueryAsync<Bot>(query)).ToList();
        }
    }
}
