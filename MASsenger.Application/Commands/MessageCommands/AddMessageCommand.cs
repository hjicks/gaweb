using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.MessageCommands
{
    public record AddMessageCommand(MessageCreateDto msg) : IRequest<TransactionResultType>;
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, TransactionResultType>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageRepository _messageRepository;
        private readonly IBaseRepository<BaseChat> _baseChatRepository;
        private readonly IBaseRepository<BaseUser> _baseUserRepository;

        public AddMessageCommandHandler(IMessageRepository messageRepository, IBaseRepository<BaseChat> baseChatRepository, IBaseRepository<BaseUser> baseUserRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _baseChatRepository = baseChatRepository;
            _baseUserRepository = baseUserRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {


            var sender = await _baseUserRepository.GetByIdAsync(request.msg.SenderID);
            if (sender == null)
                return TransactionResultType.ForeignKeyNotFound;

            var destination = await _baseChatRepository.GetByIdAsync(request.msg.DestinationID);
            if (destination == null)
                return TransactionResultType.ForeignKeyNotFound;
            var newMessage = new Message
            {
                Sender = sender,
                Destination = destination,
                Text = request.msg.Text
            };

            await _messageRepository.AddAsync(newMessage);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
