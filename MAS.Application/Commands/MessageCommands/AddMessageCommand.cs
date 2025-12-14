using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace MAS.Application.Commands.MessageCommands
{
    public record AddMessageCommand(int SenderId, MessageCreateDto Message) : IRequest<Result>;
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageRepository _messageRepository;
        private readonly IBaseRepository<BaseChat> _baseChatRepository;
        private readonly IBaseRepository<BaseUser> _baseUserRepository;
        private readonly IHubContext<ChatHub> _hubContext;

        public AddMessageCommandHandler(IMessageRepository messageRepository,
            IBaseRepository<BaseChat> baseChatRepository, IBaseRepository<BaseUser> baseUserRepository,
            IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
        {
            _messageRepository = messageRepository;
            _baseChatRepository = baseChatRepository;
            _baseUserRepository = baseUserRepository;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        public async Task<Result> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var sender = await _baseUserRepository.GetByIdAsync(request.SenderId);

            var destination = await _baseChatRepository.GetByIdAsync(request.Message.DestinationId);
            if (destination == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "Destination chat not found." });

            var newMessage = new Message
            {
                SenderId = sender!.Id,
                DestinationId = destination.Id,
                Text = request.Message.Text
            };

            await _messageRepository.AddAsync(newMessage);
            await _unitOfWork.SaveAsync();

            await _hubContext.Clients.All.SendAsync("a", sender.Name, request.Message.Text, cancellationToken: cancellationToken);

            return Result.Success(StatusCodes.Status201Created,
                new MessageReadDto
                {
                    Id = newMessage.Id,
                    SenderId = newMessage.SenderId,
                    DestinationId = newMessage.DestinationId,
                    Text = newMessage.Text,
                    CreatedAt = newMessage.CreatedAt,
                    UpdatedAt = newMessage.UpdatedAt
                });
        }
    }
}
