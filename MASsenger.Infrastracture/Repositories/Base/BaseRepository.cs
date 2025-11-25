using MASsenger.Application.Interfaces;
using MASsenger.Infrastracture.Database;

namespace MASsenger.Infrastracture.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly EfDbContext _efDbContext;
        public BaseRepository(EfDbContext efDbContext)
        {
            _efDbContext = efDbContext;
        }

        public async Task<TEntity?> GetByIdAsync(Int32 entityId)
        {
            return await _efDbContext.Set<TEntity>().FindAsync(entityId);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _efDbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _efDbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _efDbContext.Set<TEntity>().Remove(entity);
        }
    }
}
