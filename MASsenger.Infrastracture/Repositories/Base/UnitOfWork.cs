using MASsenger.Application.Interfaces;
using MASsenger.Infrastracture.Database;

namespace MASsenger.Infrastracture.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext _efDbContext;

        public UnitOfWork(EfDbContext efDbContext)
        {
            _efDbContext = efDbContext;
        }

        public async Task SaveAsync()
        {
            await _efDbContext.SaveChangesAsync();
        }
    }
}
