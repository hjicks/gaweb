using MAS.Core.Entities.ChatEntities;

namespace MAS.Application.Interfaces;

public interface IBaseChatRepository : IBaseRepository<BaseChat>
{
    Task<int> GetTypeByIdAsync(int entityId);
}
