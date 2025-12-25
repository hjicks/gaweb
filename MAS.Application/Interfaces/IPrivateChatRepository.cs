using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;

namespace MAS.Application.Interfaces;

public interface IPrivateChatRepository : IBaseRepository<PrivateChat>
{
    Task<IEnumerable<PrivateChat>> GetAllAsync();
    Task<IEnumerable<PrivateChat>> GetAllUserAsync(int userId);
    Task<IEnumerable<PrivateChatUser>> GetAllUserMembershipsAsync(int userId);
    Task<PrivateChat?> GetByIdWithMembersAsync(int pvId);
}
