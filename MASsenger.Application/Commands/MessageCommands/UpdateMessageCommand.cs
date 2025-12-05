using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Commands.MessageCommands
{
    public record UpdateMessageCommand(int SenderId, MessageUpdateDto Message) : IRequest<Result>;
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, Result>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateMessageCommandHandler(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(request.Message.Id);
            if (message == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "Message not found." });

            if (message.Sender.Id != request.SenderId)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied,
                    new[] { "You are not allowed to edit someone else's message." });

            message.Text = request.Message.Text;
            _messageRepository.Update(message);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status200OK,
                new MessageReadDto
                {
                    Text = message.Text,
                    UpdatedAt = message.UpdatedAt
                });
        }
    }
}

