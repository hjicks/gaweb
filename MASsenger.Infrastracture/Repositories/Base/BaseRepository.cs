using MASsenger.Application.Interfaces;
using MASsenger.Infrastracture.Database;

namespace MASsenger.Infrastracture.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly EfDbContext _efContext;
        public BaseRepository(EfDbContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<TEntity?> GetByIdAsync(Int32 entityId)
        {
            return await _efContext.Set<TEntity>().FindAsync(entityId);
        }

        public void Add(TEntity entity)
        {
            _efContext.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            _efContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _efContext.Set<TEntity>().Remove(entity);
        }
    }
}
