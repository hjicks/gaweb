namespace MAS.Application.Interfaces;

public interface IBaseRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(int entityId);
    Task AddAsync(TEntity entity);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);
}
