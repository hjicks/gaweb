using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Commands.MessageCommands
{
    public record AddMessageCommand(int SenderId, MessageCreateDto Message) : IRequest<Result>;
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageRepository _messageRepository;
        private readonly IBaseRepository<BaseChat> _baseChatRepository;
        private readonly IBaseRepository<BaseUser> _baseUserRepository;

        public AddMessageCommandHandler(IMessageRepository messageRepository,
            IBaseRepository<BaseChat> baseChatRepository, IBaseRepository<BaseUser> baseUserRepository,
            IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _baseChatRepository = baseChatRepository;
            _baseUserRepository = baseUserRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var sender = await _baseUserRepository.GetByIdAsync(request.SenderId);

            var destination = await _baseChatRepository.GetByIdAsync(request.Message.DestinationID);
            if (destination == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "Destination chat not found." });

            var newMessage = new Message
            {
                Sender = sender!,
                Destination = destination,
                Text = request.Message.Text
            };

            await _messageRepository.AddAsync(newMessage);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status201Created,
                new MessageReadDto
                {
                    Id = newMessage.Id,
                    SenderID = newMessage.Sender.Id,
                    DestinationID = newMessage.Destination.Id,
                    Text = newMessage.Text,
                    CreatedAt = newMessage.CreatedAt,
                    UpdatedAt = newMessage.UpdatedAt
                });
        }
    }
}
