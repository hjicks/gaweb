using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetAllAsync();
    }
}
