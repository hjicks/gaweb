using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;

namespace MASsenger.Infrastracture.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly DapperDbContext _dapperDbContext;
        public UserRepository(EfDbContext efContext, DapperDbContext dapperDbContext) : base(efContext)
        {
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseUsers WHERE Type == 'User'";
            return (await _dapperDbContext.GetConnection().QueryAsync<User>(query)).ToList();
        }
        public async Task<Boolean> Authenticate(Int32 id, string Passwd)
        {
            /* we could have also written this as a query. */
            string query = "SELECT * FROM BaseUsers WHERE Type == 'User' AND ID == @id";
            return (await _dapperDbContext.GetConnection().QueryAsync<String>(query)).Equals(Passwd);

        }
    }
}
