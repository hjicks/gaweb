using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.UserEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
    {

    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        string query = "SELECT * FROM Users";
        return (await _dapperDbContext.GetConnection().QueryAsync<User>(query)).ToList();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _efDbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdWithPrivateChatsAsync(int userId)
    {
        return await _efDbContext.Users
            .Where(u => u.Id == userId)
            .Include(u => u.PrivateChats)
            .SingleOrDefaultAsync();
    }

    public async Task<bool> IsExistsAsync(int userId)
    {
        return await _efDbContext.Users.AnyAsync(u => u.Id == userId);
    }

    public async Task<bool> IsExistsAsync(string username)
    {
        return await _efDbContext.Users.AnyAsync(u => u.Username == username);
    }
}
