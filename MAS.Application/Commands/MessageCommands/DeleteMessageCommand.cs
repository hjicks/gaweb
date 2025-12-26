using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Constants;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Commands.MessageCommands;

public record DeleteMessageCommand(int UserId, int MessageId) : IRequest<Result>;
public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IBaseChatRepository _baseChatRepository;
    private readonly IBaseRepository<GroupChatUser> _groupChatUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteMessageCommandHandler(IMessageRepository messageRepository, IBaseChatRepository baseChatRepository,
        IBaseRepository<GroupChatUser> groupChatUserRepository, IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _baseChatRepository = baseChatRepository;
        _groupChatUserRepository = groupChatUserRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.MessageNotFound);

        var chatType = await _baseChatRepository.GetTypeByIdAsync(message.DestinationId);
        if (chatType == (int)ChatType.Private && message.SenderId != request.UserId)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        if (chatType == (int)ChatType.Group)
        {
            var member = await _groupChatUserRepository.GetByIdAsync(request.UserId);
            if (member == null)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);
            if (member.Role == GroupChatRole.Member && message.SenderId != member.MemberId)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);
        }

        message.IsDeleted = true;
        
        _messageRepository.Update(message);
        await _unitOfWork.SaveAsync();

        Log.Information($"User {request.UserId} deleted message {message.Id}.");
        return Result.Success(StatusCodes.Status200OK,
            ResponseMessages.Success[SuccessType.DeleteSuccessful]);
    }
}
