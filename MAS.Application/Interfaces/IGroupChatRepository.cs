using MAS.Core.Entities.ChatEntities;

namespace MAS.Application.Interfaces
{
    public interface IGroupChatRepository : IBaseRepository<GroupChat>
    {
        Task<IEnumerable<GroupChat>> GetAllAsync();
        Task<GroupChat?> IncludedGetByIdAsync(int groupId);
    }
}
