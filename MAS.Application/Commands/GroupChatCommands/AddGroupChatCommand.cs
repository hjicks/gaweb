using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Enums;
using MediatR;

namespace MAS.Application.Commands.GroupChatCommands
{
    public record AddGroupChatCommand(PublicGroupChatAddDto GroupChat, Int32 OwnerId) : IRequest<TransactionResultType>;
    public class AddChannelChatCommandHandler : IRequestHandler<AddGroupChatCommand, TransactionResultType>
    {
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddChannelChatCommandHandler(IGroupChatRepository groupChatRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _groupChatRepository = groupChatRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(AddGroupChatCommand request, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.OwnerId);
            if (owner == null)
                return TransactionResultType.ForeignKeyNotFound;
            var newChannelChat = new GroupChat
            {
                DisplayName = request.GroupChat.DisplayName,
                Groupname = request.GroupChat.Groupname,
                Description = request.GroupChat.Description,
            };
            //newChannelChat.OwnerId = owner.Id;
            await _groupChatRepository.AddAsync(newChannelChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
