using MAS.Core.Entities.UserEntities;

namespace MAS.Application.Interfaces
{
    public interface IBotRepository : IBaseRepository<Bot>
    {
        Task<IEnumerable<Bot>> GetAllAsync();
    }
}
