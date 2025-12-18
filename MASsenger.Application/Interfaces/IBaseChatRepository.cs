using MASsenger.Core.Entities.ChatEntities;

namespace MASsenger.Application.Interfaces
{
    public interface IBaseChatRepository : IBaseRepository<BaseChat>
    {
        Task<string> GetTypeByIdAsync(Int32 entityId);
    }
}
