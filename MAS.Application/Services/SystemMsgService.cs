using MAS.Application.Interfaces;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Enums;

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

    public async Task SendSystemMsgAsync(int chatId, MasEvent masEvent, int userId)
    {
        var username = await _userRepository.GetUsernameByIdAsync(userId);
        string message = string.Empty;
        switch (masEvent)
        {
            case MasEvent.Join:
                message = $"{username} joined the group.";
                break;
            case MasEvent.Leave:
                message = $"{username} left the group.";
                break;
            case MasEvent.Ban:
                message = $"{username} is banned from the group.";
                break;
            case MasEvent.Promote:
                message = $"{username} is now admin of the group.";
                break;
            case MasEvent.Demote:
                message = $"{username} is no longer admin of the group.";
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
