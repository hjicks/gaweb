using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.SystemMessageCommands
{
    public record AddSystemMessageCommand(SystemMessageCreateDto msg) : IRequest<TransactionResultType>;
    public class AddSystemMessageCommandHandler : IRequestHandler<AddSystemMessageCommand, TransactionResultType>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemMessageRepository _messageRepository;
        private readonly IBaseRepository<BaseChat> _baseChatRepository;

        public AddSystemMessageCommandHandler(ISystemMessageRepository messageRepository, IBaseRepository<BaseChat> baseChatRepository, IUnitOfWork unitOfWork)
        {
            _messageRepository = messageRepository;
            _baseChatRepository = baseChatRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(AddSystemMessageCommand request, CancellationToken cancellationToken)
        {
            var destination = await _baseChatRepository.GetByIdAsync(request.msg.DestinationID);
            if (destination == null)
                return TransactionResultType.ForeignKeyNotFound;
            var newMessage = new SystemMessage
            {
                Destination = destination,
                Text = request.msg.Text,
                SentTime = request.msg.SentTime
            };

            await _messageRepository.Add(newMessage);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
