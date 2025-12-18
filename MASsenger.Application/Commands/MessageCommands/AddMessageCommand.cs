using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Hubs;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace MASsenger.Application.Commands.MessageCommands
{
    public record AddMessageCommand(int SenderId, MessageCreateDto Message) : IRequest<Result>;
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageRepository _messageRepository;
        private readonly IBaseChatRepository _baseChatRepository;
        private readonly IBaseRepository<BaseUser> _baseUserRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IChannelChatRepository _channelChatRepository;

        public AddMessageCommandHandler(IMessageRepository messageRepository,
            IBaseChatRepository baseChatRepository, IBaseRepository<BaseUser> baseUserRepository,
            IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext, IPrivateChatRepository privateChatRepository, IChannelChatRepository channelChatRepository)
        {
            _messageRepository = messageRepository;
            _baseChatRepository = baseChatRepository;
            _baseUserRepository = baseUserRepository;
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _privateChatRepository = privateChatRepository;
            _channelChatRepository = channelChatRepository;
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

            MessageReadDto result = new MessageReadDto
            {
                Id = newMessage.Id,
                SenderId = newMessage.SenderId,
                DestinationId = newMessage.DestinationId,
                Text = newMessage.Text,
                CreatedAt = newMessage.CreatedAt,
                UpdatedAt = newMessage.UpdatedAt
            };

            if (_baseChatRepository.GetTypeByIdAsync(request.Message.DestinationId).Result == "Private")
            {
                PrivateChat pc = await _privateChatRepository.GetByIdAsync(destination.Id);
                int dstUserId = sender.Id == pc.StarterId ? pc.ReceiverId : pc.StarterId;
                BaseUser u = await _baseUserRepository.GetByIdAsync(dstUserId);
                await _hubContext.Clients.User(u.Id.ToString()).SendAsync("AddMessage",
                    result, cancellationToken: cancellationToken);
            }
            else if (_baseChatRepository.GetTypeByIdAsync(request.Message.DestinationId).Result == "Channel")
            {
                ChannelChat cc = await _channelChatRepository.GetByIdAsync(destination.Id);
                foreach (User u in cc.Members)
                {
                    if (u.Id == sender.Id)
                        continue;
                    
                    await _hubContext.Clients.User(u.Id.ToString()).SendAsync("AddMessage",
                        result, cancellationToken: cancellationToken);
                }
            }

                return Result.Success(StatusCodes.Status201Created, result);
        }
    }
}
