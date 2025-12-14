using MAS.Core.Entities.MessageEntities;

namespace MAS.Application.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetAllAsync();

    }
}
