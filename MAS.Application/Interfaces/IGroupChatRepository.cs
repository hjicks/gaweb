using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;

namespace MAS.Application.Interfaces;

public interface IGroupChatRepository : IBaseRepository<GroupChat>
{
    Task<IEnumerable<GroupChat>> GetAllAsync();
    Task<IEnumerable<GroupChat>> GetAllUserAsync(int userId);
    Task<GroupChat?> GetByGroupnameAsync(string groupname);
    Task<GroupChat?> GetByIdWithMemberAsync(int userId, int groupId);
    Task<GroupChat?> GetByIdWithMembersAsync(int groupId);
    Task<GroupChatUser?> GetMemberAsync(int groupId, int memberId);
    Task<bool> IsExistsAsync(string groupname);
    Task<bool> IsMemberExistsAsync(int groupId, int memberId);
}
