using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
{
    public class PrivateChatRepository : BaseRepository<PrivateChat>, IPrivateChatRepository
    {
        private readonly DapperDbContext _dapperDbContext;
        public PrivateChatRepository(EfDbContext efContext, DapperDbContext dapperDbContext) : base(efContext)
        {
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<PrivateChat>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseChats WHERE Type == 'PrivateChat'";
            return (await _dapperDbContext.GetConnection().QueryAsync<PrivateChat>(query)).ToList();
        }
    }
}
