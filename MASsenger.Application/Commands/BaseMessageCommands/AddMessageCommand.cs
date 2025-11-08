using MASsenger.Application.Dto.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.Message;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BaseMessageCommands
{
    public record AddMessageCommand(MessageCreateDto msg) : IRequest<TransactionResultType>;
    public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, TransactionResultType>
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        private readonly IBaseChatRepository _baseChatRepository;
        private readonly IBaseUserRepository _baseUserRepository;
        public AddMessageCommandHandler(IBaseMessageRepository baseMessageRepository, IBaseChatRepository baseChatRepository, IBaseUserRepository baseUserRepository)
        {
            _baseMessageRepository = baseMessageRepository;
            _baseChatRepository = baseChatRepository;
            _baseUserRepository = baseUserRepository;
        }
        public async Task<TransactionResultType> Handle(AddMessageCommand request, CancellationToken cancellationToken)
        {
            var sender = await _baseUserRepository.GetUserByIdAsync(request.msg.SenderID);
            if (sender == null)
                return TransactionResultType.ForeignKeyNotFound;

            var destination = await _baseChatRepository.GetBaseChatByIdAsync(request.msg.DestinationID);
            if (destination == null)
                return TransactionResultType.ForeignKeyNotFound;

            var newMessage = new Message
            {
                Sender = sender,
                Destination = destination,
                Text = request.msg.Text,
                SentTime = request.msg.SentTime
            };

            if (await _baseMessageRepository.AddMessageAsync(newMessage))
                return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
