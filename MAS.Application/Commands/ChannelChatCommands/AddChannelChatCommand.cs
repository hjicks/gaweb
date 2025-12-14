using MAS.Application.Dtos.ChannelChatDtos;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Enums;
using MediatR;

namespace MAS.Application.Commands.ChannelChatCommands
{
    public record AddChannelChatCommand(ChannelChatCreateDto channelChat, Int32 ownerId) : IRequest<TransactionResultType>;
    public class AddChannelChatCommandHandler : IRequestHandler<AddChannelChatCommand, TransactionResultType>
    {
        private readonly IChannelChatRepository _channelChatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddChannelChatCommandHandler(IChannelChatRepository channelChatRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _channelChatRepository = channelChatRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(AddChannelChatCommand request, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.ownerId);
            if (owner == null)
                return TransactionResultType.ForeignKeyNotFound;
            var newChannelChat = new ChannelChat
            {
                Name = request.channelChat.Name,
                Username = request.channelChat.Username,
                Description = request.channelChat.Description,
            };
            newChannelChat.OwnerId = owner.Id;
            await _channelChatRepository.AddAsync(newChannelChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
