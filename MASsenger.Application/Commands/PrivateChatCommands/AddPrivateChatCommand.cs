using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.PrivateChatCommands
{
    public record AddPrivateChatCommand() : IRequest<TransactionResultType>;
    public class AddPrivateChatCommandHandler : IRequestHandler<AddPrivateChatCommand, TransactionResultType>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddPrivateChatCommandHandler(IPrivateChatRepository privateChatRepository, IUnitOfWork unitOfWork)
        {
            _privateChatRepository = privateChatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(AddPrivateChatCommand request, CancellationToken cancellationToken)
        {
            var newPrivateChat = new PrivateChat();
            await _privateChatRepository.AddAsync(newPrivateChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
