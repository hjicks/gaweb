using MAS.Core.Entities.MessageEntities;

namespace MAS.Application.Interfaces
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Task<IEnumerable<Message>> GetAllAsync();
        Task<IEnumerable<Message>> GetAllChatAsync(int chatId);
        Task<Message?> GetChatLastMessageAsync(int chatId);
        Task<IEnumerable<Message>> GetChatLastMessagesAsync(int chatId, DateTime createdAt, ushort count);

    }
}
