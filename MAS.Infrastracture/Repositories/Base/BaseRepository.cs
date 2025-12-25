using MAS.Application.Interfaces;
using MAS.Infrastracture.Database;

namespace MAS.Infrastracture.Repositories.Base;

public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    protected readonly EfDbContext _efDbContext;
    protected readonly DapperDbContext _dapperDbContext;
    public BaseRepository(EfDbContext efDbContext, DapperDbContext dapperDbContext)
    {
        _efDbContext = efDbContext;
        _dapperDbContext = dapperDbContext;
    }

    public async Task<TEntity?> GetByIdAsync(int entityId)
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

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _efDbContext.Set<TEntity>().UpdateRange(entities);
    }

    public void Delete(TEntity entity)
    {
        _efDbContext.Set<TEntity>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        _efDbContext.Set<TEntity>().RemoveRange(entities);
    }
}
