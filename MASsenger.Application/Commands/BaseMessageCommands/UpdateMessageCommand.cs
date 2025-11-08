using MASsenger.Application.Dto.Update;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BaseMessageCommands
{
    public record UpdateMessageCommand(MessageUpdateDto msg) : IRequest<TransactionResultType>;
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, TransactionResultType>
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        public UpdateMessageCommandHandler(IBaseMessageRepository baseMessageRepository)
        {
            _baseMessageRepository = baseMessageRepository;
        }
        public async Task<TransactionResultType> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var msg = await _baseMessageRepository.GetMessageByIdAsync(request.msg.Id);
            if (msg == null)
                return TransactionResultType.ForeignKeyNotFound;

            msg.Text = request.msg.Text;

            if (await _baseMessageRepository.UpdateMessageAsync(msg)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
