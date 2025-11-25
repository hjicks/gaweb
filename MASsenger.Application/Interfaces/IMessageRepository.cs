using MASsenger.Core.Entities.MessageEntities;

namespace MASsenger.Application.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetAllAsync();

    }
}
