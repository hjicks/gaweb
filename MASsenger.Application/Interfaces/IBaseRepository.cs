namespace MASsenger.Application.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        Task<TEntity?> GetByIdAsync(Int32 entityId);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
