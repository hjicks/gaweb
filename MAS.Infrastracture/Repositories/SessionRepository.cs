using Dapper;
using MAS.Application.Interfaces;
using MAS.Core.Entities.UserEntities;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MAS.Infrastracture.Repositories;

public class SessionRepository : BaseRepository<Session>, ISessionRepository
{
    public SessionRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext) : base(efDbContext, dapperDbContext)
    {

    }

    public async Task<IEnumerable<Session>> GetAllAsync()
    {
        string query = "SELECT * FROM Sessions";
        return (await _dapperDbContext.GetConnection().QueryAsync<Session>(query)).ToList();
    }

    public async Task<bool> GetActiveAsync(int userId)
    {
        return await _efDbContext.Sessions
            .Where(s => s.UserId == userId && s.IsRevoked == false)
            .AnyAsync();
    }

    public async Task<Session?> GetByTokenAsync(byte[] sessionRefreshTokenHash)
    {
        return await _efDbContext.Sessions
            .Where(s => s.TokenHash == sessionRefreshTokenHash && s.IsDeleted == false)
            .SingleOrDefaultAsync();
    }
}
