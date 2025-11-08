using MASsenger.Core.Entities;
using MASsenger.Core.Entities.Message;

namespace MASsenger.Application.Interfaces
{
    public interface IBaseMessageRepository
    {
         Task<IEnumerable<SystemMessage>> GetAllSystemMessagesAsync();
        Task<bool> AddSystemMessageAsync(SystemMessage systemMessage);
        Task<SystemMessage?> GetSystemMessageByIdAsync(UInt64 msgId);
        Task<bool> UpdateSystemMessageAsync(SystemMessage message);

        Task<IEnumerable<Message>> GetAllMessagesAsync();
        Task<bool> AddMessageAsync(Message baseMessage);
        Task<Message?> GetMessageByIdAsync(UInt64 msgId);
        Task<bool> UpdateMessageAsync(Message message);
        Task<bool> DeleteMessageAsync(Message message);

        Task<bool> Save();
    }
}
