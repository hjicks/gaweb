using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
{
    public class PrivateChatRepository : BaseRepository<PrivateChat>, IPrivateChatRepository
    {
        private readonly DapperDbContext _dapperDbContext;
        public PrivateChatRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext)
        {
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<PrivateChat>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseChats WHERE Type == 'Private'";
            return (await _dapperDbContext.GetConnection().QueryAsync<PrivateChat>(query)).ToList();
        }
    }
}
