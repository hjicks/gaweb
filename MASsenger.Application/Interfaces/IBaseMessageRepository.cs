using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IBaseMessageRepository
    {
        Task<IEnumerable<BaseMessage>> GetBaseMessagesAsync();
        Task<bool> AddBaseMessageAsync(BaseMessage baseMessage, User sender, BaseChat destinationChat);
        Task<bool> Save();
    }
}
