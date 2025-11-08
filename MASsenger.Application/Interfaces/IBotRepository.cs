using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IBotRepository : IBaseRepository<Bot>
    {
        Task<IEnumerable<Bot>> GetAllAsync();
    }
}
