using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace MAS.Application.Commands.GroupChatCommands;

public record LeavePrivateChatCommand(int UserId, int PrivateChatId) : IRequest<Result>;
public class LeavePrivateChatCommandHandler : IRequestHandler<LeavePrivateChatCommand, Result>
{
    private readonly IPrivateChatRepository _privateChatRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    public LeavePrivateChatCommandHandler(IPrivateChatRepository privateChatRepository,
        IMessageRepository messageRepository, IUnitOfWork unitOfWork)
    {
        _privateChatRepository = privateChatRepository;
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(LeavePrivateChatCommand request, CancellationToken cancellationToken)
    {
        var privateChat = await _privateChatRepository.GetByIdWithMembersAsync(request.PrivateChatId);
        if (privateChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (!privateChat.Members.Any(m => m.Id == request.UserId))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var member = privateChat.Members.Single(m => m.Id == request.UserId);
        privateChat.Members.Remove(member);

        if (privateChat.Members.IsNullOrEmpty())
        {
            privateChat.IsDeleted = true;

            var pvMessages = await _messageRepository.GetAllChatAsync(request.PrivateChatId);
            foreach (var message in pvMessages)
                message.IsDeleted = true;
            _messageRepository.UpdateRange(pvMessages);
        }

        _privateChatRepository.Update(privateChat);
        await _unitOfWork.SaveAsync();

        return Result.Success(StatusCodes.Status200OK, "Member removed from the chat successfully.");
    }
}
