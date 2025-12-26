using MAS.Application.Interfaces;
using MAS.Core.Entities.MessageEntities;

namespace MAS.Application.Services;

public class SystemMsgService : ISystemMsgService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBaseChatRepository _baseChatRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SystemMsgService(IMessageRepository messageRepository, IUserRepository userRepository,
        IBaseChatRepository baseChatRepository, IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _baseChatRepository = baseChatRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task SendSystemMsgAsync(int chatId, string masEvent, string username)
    {
        string message = string.Empty;
        switch (masEvent)
        {
            case "Join":
                message = $"{username} joined the group.";
                break;
            case "Leave":
                message = $"{username} left the group.";
                break;
            case "Ban":
                message = $"{username} is banned from the group.";
                break;
            case "Promote":
                message = $"{username} is now admin of the group.";
                break;
        }

        var sysAdmin = await _userRepository.GetByIdAsync(1);
        var destinationChat = await _baseChatRepository.GetByIdAsync(chatId);
        var sysMsg = new Message()
        {
            Sender = sysAdmin!,
            Destination = destinationChat!,
            Text = message
        };
        await _messageRepository.AddAsync(sysMsg);
        await _unitOfWork.SaveAsync();
    }
}
