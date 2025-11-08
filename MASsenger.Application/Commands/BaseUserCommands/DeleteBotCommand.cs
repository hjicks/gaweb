using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BaseUserCommands
{
    public record DeleteBotCommand(ulong botId) : IRequest<TransactionResultType>;
    public class DeleteBotCommandHandler : IRequestHandler<DeleteBotCommand, TransactionResultType>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public DeleteBotCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }

        public async Task<TransactionResultType> Handle(DeleteBotCommand request, CancellationToken cancellationToken)
        {
            var bot = await _baseUserRepository.GetBotByIdAsync(request.botId);
            if (bot == null)
                return TransactionResultType.ForeignKeyNotFound;
            if (await _baseUserRepository.DeleteBotAsync(bot)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
