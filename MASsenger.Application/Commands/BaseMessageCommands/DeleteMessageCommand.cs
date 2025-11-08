using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BaseMessageCommands
{
    public record DeleteMessageCommand(ulong msgId) : IRequest<TransactionResultType>;
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, TransactionResultType>
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        public DeleteMessageCommandHandler(IBaseMessageRepository baseMessageRepository)
        {
            _baseMessageRepository = baseMessageRepository;
        }

        public async Task<TransactionResultType> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var msg = await _baseMessageRepository.GetMessageByIdAsync(request.msgId);
            if (msg == null)
                return TransactionResultType.ForeignKeyNotFound;
            if (await _baseMessageRepository.DeleteMessageAsync(msg)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
