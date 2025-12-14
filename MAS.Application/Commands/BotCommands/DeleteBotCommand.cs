using MAS.Application.Interfaces;
using MAS.Core.Enums;
using MediatR;

namespace MAS.Application.Commands.BotCommands
{
    public record DeleteBotCommand(Int32 botId) : IRequest<TransactionResultType>;
    public class DeleteBotCommandHandler : IRequestHandler<DeleteBotCommand, TransactionResultType>
    {
        private readonly IBotRepository _botRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteBotCommandHandler(IBotRepository botRepository, IUnitOfWork unitOfWork)
        {
            _botRepository = botRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TransactionResultType> Handle(DeleteBotCommand request, CancellationToken cancellationToken)
        {
            var bot = await _botRepository.GetByIdAsync(request.botId);
            if (bot == null)
                return TransactionResultType.ForeignKeyNotFound;
            _botRepository.Delete(bot);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
