namespace MASsenger.Application.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity?> GetByIdAsync(UInt64 entityId);
        IQueryable<TEntity> GetQueryable();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
