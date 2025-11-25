using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace MASsenger.Infrastracture.Repositories
{
    public class SessionRepository : BaseRepository<Session>, ISessionRepository
    {
        private readonly EfDbContext  _efDbContext;
        public SessionRepository(EfDbContext efDbContext) : base(efDbContext)
        {
            _efDbContext = efDbContext;
        }

        public async Task<IEnumerable<Session>> GetAllAsync()
        {
            return await _efDbContext.Sessions.ToListAsync();
        }
    }
}
