using MASsenger.Core.Entities.UserEntities;

namespace MASsenger.Application.Interfaces
{
    public interface IBotRepository : IBaseRepository<Bot>
    {
        Task<IEnumerable<Bot>> GetAllAsync();
    }
}
