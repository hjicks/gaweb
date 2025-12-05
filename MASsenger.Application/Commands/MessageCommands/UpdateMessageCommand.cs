using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.MessageCommands
{
    public record UpdateMessageCommand(MessageUpdateDto message) : IRequest<TransactionResultType>;
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, TransactionResultType>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateMessageCommandHandler(IMessageRepository messageRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetByIdAsync(request.message.Id);
            if (message == null)
                return TransactionResultType.ForeignKeyNotFound;
            message.Text = request.message.Text;
            _messageRepository.Update(message);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}

