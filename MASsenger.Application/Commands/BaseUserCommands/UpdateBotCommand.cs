using MASsenger.Core.Dto.Update;
using MASsenger.Core.Enums;
using MASsenger.Core.Interfaces;
using MediatR;

namespace MASsenger.Application.Commands.BaseUserCommands
{
    public record UpdateBotCommand(BotUpdateDto bot) : IRequest<TransactionResultType>;
    public class UpdateBotCommandHandler : IRequestHandler<UpdateBotCommand, TransactionResultType>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public UpdateBotCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<TransactionResultType> Handle(UpdateBotCommand request, CancellationToken cancellationToken)
        {
            var bot = await _baseUserRepository.GetBotByIdAsync(request.bot.Id);
            if (bot == null)
                return TransactionResultType.ForeignKeyNotFound;
            bot.Name = request.bot.Name;
            bot.Username = request.bot.Username;
            bot.Description = request.bot.Description;
            if (await _baseUserRepository.UpdateBotAsync(bot)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
