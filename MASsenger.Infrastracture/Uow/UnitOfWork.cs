using MASsenger.Application.Interfaces;
using MASsenger.Infrastracture.Data;

namespace MASsenger.Infrastracture.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MessengerDbContext _context;

        public UnitOfWork(MessengerDbContext context)
        {
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
