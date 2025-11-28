using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.PrivateChatCommands
{
    public record AddPrivateChatCommand(Int32 starterId,Int32 receiverId) : IRequest<TransactionResultType>;
    public class AddPrivateChatCommandHandler : IRequestHandler<AddPrivateChatCommand, TransactionResultType>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddPrivateChatCommandHandler(IPrivateChatRepository privateChatRepository,IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _privateChatRepository = privateChatRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(AddPrivateChatCommand request, CancellationToken cancellationToken)
        {
            var starter = await _userRepository.GetByIdAsync(request.starterId);
            var receiver = await _userRepository.GetByIdAsync(request.receiverId);

            if (starter == null || receiver == null)
                return TransactionResultType.ForeignKeyNotFound;
            
            var newPrivateChat = new PrivateChat
            {
                Starter = starter,
                Receiver = receiver
            };

            await _privateChatRepository.AddAsync(newPrivateChat);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
