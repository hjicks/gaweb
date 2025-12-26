using MAS.Core.Enums;

namespace MAS.Application.Interfaces;

public interface ISystemMsgService
{
    Task SendSystemMsgAsync(int chatId, MasEvent masEvent, int userId);
}
