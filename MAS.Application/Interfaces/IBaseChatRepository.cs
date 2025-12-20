using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;

namespace MAS.Application.Interfaces
{
    public interface IBaseChatRepository : IBaseRepository<BaseChat>
    {
        Task<string> GetTypeByIdAsync(Int32 entityId);
    }
}
