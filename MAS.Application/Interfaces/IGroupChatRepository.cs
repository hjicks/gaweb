using MAS.Core.Entities.ChatEntities;

namespace MAS.Application.Interfaces;

public interface IGroupChatRepository : IBaseRepository<GroupChat>
{
    Task<IEnumerable<GroupChat>> GetAllAsync();
    Task<GroupChat?> GetByGroupnameAsync(string groupname);
    Task<GroupChat?> GetByIdWithMembersAsync(int groupId);
    Task<bool> IsExistsAsync(string groupname);
}
