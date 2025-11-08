using MASsenger.Application.Dto.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.Message;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BaseMessageCommands
{
    public record AddSystemMessageCommand(SystemMessageCreateDto systemMessage) : IRequest<TransactionResultType>;
    public class AddSystemMessageCommandHandler : IRequestHandler<AddSystemMessageCommand, TransactionResultType>
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        private readonly IBaseChatRepository _baseChatRepository;
        public AddSystemMessageCommandHandler(IBaseMessageRepository baseMessageRepository, IBaseChatRepository baseChatRepository)
        {
            _baseMessageRepository = baseMessageRepository;
            _baseChatRepository = baseChatRepository;
        }
        public async Task<TransactionResultType> Handle(AddSystemMessageCommand request, CancellationToken cancellationToken)
        {
            var destination = await _baseChatRepository.GetBaseChatByIdAsync(request.systemMessage.DestinationID);
            if (destination == null)
                return TransactionResultType.ForeignKeyNotFound;

            var newSystemMessage = new SystemMessage
            {
                Destination = destination,
                Text = request.systemMessage.Text,
                SentTime = request.systemMessage.SentTime
            };

            if (await _baseMessageRepository.AddSystemMessageAsync(newSystemMessage))
                return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
