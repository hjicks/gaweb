using MASsenger.Application.Dtos.BotDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BotCommands
{
    public record UpdateBotCommand(BotUpdateDto bot) : IRequest<TransactionResultType>;
    public class UpdateBotCommandHandler : IRequestHandler<UpdateBotCommand, TransactionResultType>
    {
        private readonly IBotRepository _botRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateBotCommandHandler(IBotRepository botRepository, IUnitOfWork unitOfWork)
        {
            _botRepository = botRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(UpdateBotCommand request, CancellationToken cancellationToken)
        {
            var bot = await _botRepository.GetByIdAsync(request.bot.Id);
            if (bot == null)
                return TransactionResultType.ForeignKeyNotFound;
            bot.Name = request.bot.Name;
            bot.Username = request.bot.Username;
            bot.Description = request.bot.Description;
            _botRepository.Update(bot);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
