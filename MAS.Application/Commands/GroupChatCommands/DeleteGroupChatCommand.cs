using MAS.Application.Interfaces;
using MAS.Core.Enums;
using MediatR;

namespace MAS.Application.Commands.GroupChatCommands
{
    public record DeleteGroupChatCommand(Int32 GroupChatId) : IRequest<TransactionResultType>;
    public class DeleteChannelChatCommandHandler : IRequestHandler<DeleteGroupChatCommand, TransactionResultType>
    {
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteChannelChatCommandHandler(IGroupChatRepository groupChatRepository, IUnitOfWork unitOfWork)
        {
            _groupChatRepository = groupChatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(DeleteGroupChatCommand request, CancellationToken cancellationToken)
        {
            var groupChat = await _groupChatRepository.GetByIdAsync(request.GroupChatId);
            if (groupChat == null)
                return TransactionResultType.ForeignKeyNotFound;
            _groupChatRepository.Delete(groupChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
