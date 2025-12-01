using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
        {

        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseUsers WHERE Type == 'User'";
            return (await _dapperDbContext.GetConnection().QueryAsync<User>(query)).ToList();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _efDbContext.Users.Where(u => u.Username == username).SingleOrDefaultAsync();
        }
    }
}
