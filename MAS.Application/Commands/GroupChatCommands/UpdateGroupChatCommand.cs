using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Core.Enums;
using MediatR;

namespace MAS.Application.Commands.GroupChatCommands
{
    public record UpdateGroupChatCommand(PublicGroupChatUpdateDto GroupChat) : IRequest<TransactionResultType>;
    public class UpdateChannelChatCommandHandler : IRequestHandler<UpdateGroupChatCommand, TransactionResultType>
    {
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateChannelChatCommandHandler(IGroupChatRepository groupChatRepository, IUnitOfWork unitOfWork)
        {
            _groupChatRepository = groupChatRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(UpdateGroupChatCommand request, CancellationToken cancellationToken)
        {
            var groupChat = await _groupChatRepository.GetByIdAsync(request.GroupChat.Id);
            if (groupChat == null)
                return TransactionResultType.ForeignKeyNotFound;
            groupChat.DisplayName = request.GroupChat.DisplayName;
            groupChat.Groupname = request.GroupChat.Groupname;
            groupChat.Description = request.GroupChat.Description;
            _groupChatRepository.Update(groupChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
