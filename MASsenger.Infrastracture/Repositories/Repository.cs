using Dapper;
using MASsenger.Application.Interfaces;
using MASsenger.Infrastracture.Data;

namespace MASsenger.Infrastracture.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly MessengerDbContext _context;
        private readonly DapperDbContext _dapperDbContext;
        public Repository(MessengerDbContext context, DapperDbContext dapperDbContext)
        {
            _context = context;
            _dapperDbContext = dapperDbContext;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            string query = "SELECT * FROM BaseUsers WHERE Type == 'User'";
            return (await _dapperDbContext.GetConnection().QueryAsync<TEntity>(query)).ToList();
        }

        public async Task<TEntity?> GetByIdAsync(UInt64 entityId)
        {
            return await _context.Set<TEntity>().FindAsync(entityId);
        }

        public IQueryable<TEntity> GetQueryable()
        {
            return _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }
    }
}
