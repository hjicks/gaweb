using MASsenger.Application.Dtos.SystemMessageDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.MessageEntities;
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
            var destination = await _baseChatRepository.GetByIdAsync(request.msg.DestinationId);
            if (destination == null)
                return TransactionResultType.ForeignKeyNotFound;
            var newMessage = new SystemMessage
            {
                DestinationId = destination.Id,
                Text = request.msg.Text
            };

            await _messageRepository.AddAsync(newMessage);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
