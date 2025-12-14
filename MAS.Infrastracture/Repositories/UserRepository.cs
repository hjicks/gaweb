using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.UserEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Repositories
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

        public async Task<bool> IsExistsAsync(Int32 userId)
        {
            return await _efDbContext.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
