using MAS.Core.Entities.ChatEntities;

namespace MAS.Application.Interfaces;

public interface IPrivateChatRepository : IBaseRepository<PrivateChat>
{
    Task<IEnumerable<PrivateChat>> GetAllAsync();
    Task<IEnumerable<PrivateChat>> GetAllUserAsync(int userId);
    Task<PrivateChat?> GetByIdWithMembersAsync(int pvId);
}
