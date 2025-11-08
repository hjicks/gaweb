using MASsenger.Application.Interfaces;
using MASsenger.Infrastracture.Data;

namespace MASsenger.Infrastracture.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfDbContext _context;

        public UnitOfWork(EfDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
