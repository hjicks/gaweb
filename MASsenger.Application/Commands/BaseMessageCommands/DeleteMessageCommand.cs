using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BaseMessageCommands
{
    public record DeleteMessageCommand(Int32 messageId) : IRequest<TransactionResultType>;
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, TransactionResultType>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteMessageCommandHandler(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(request.messageId);
            if (message == null)
                return TransactionResultType.ForeignKeyNotFound;
            _messageRepository.Delete(message);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
