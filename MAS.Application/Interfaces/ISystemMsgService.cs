namespace MAS.Application.Interfaces
{
    public interface ISystemMsgService
    {
        Task SendSystemMsgAsync(int chatId, string masEvent, string username);
    }
}
