using MAS.Application.Interfaces;
using MAS.Infrastracture.Database;

namespace MAS.Infrastracture.Repositories.Base
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
