using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.MessageCommands;

public record DeleteMessageCommand(int SenderId, int MessageId) : IRequest<Result>;
public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, Result>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteMessageCommandHandler(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
    {
        _messageRepository = messageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                new[] { "Message not found." });

        if (message.SenderId != request.SenderId)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied,
                new[] { "You are not allowed to delete someone else's message." });

        _messageRepository.Delete(message);
        await _unitOfWork.SaveAsync();

        return Result.Success(StatusCodes.Status200OK);
    }
}
