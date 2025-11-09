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
    }
}
