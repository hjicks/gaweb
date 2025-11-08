using MASsenger.Core.Entities;

namespace MASsenger.Application.Interfaces
{
    public interface IBaseMessageRepository
    {
        Task<IEnumerable<BaseMessage>> GetBaseMessagesAsync();
        Task<bool> AddBaseMessageAsync(BaseMessage baseMessage);
        Task<BaseMessage?> GetBaseMessageByIdAsync(UInt64 msgId);
        Task<bool> UpdateBaseMessageAsync(BaseMessage message);
        Task<bool> DeleteBaseMessageAsync(BaseMessage message);
        Task<bool> Save();
    }
}
