using MASsenger.Application.Dto.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BaseUserCommands
{
    public record AddBotCommand(BotCreateDto bot, ulong ownerId) : IRequest<TransactionResultType>;
    public class AddBotCommandHandler : IRequestHandler<AddBotCommand, TransactionResultType>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public AddBotCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<TransactionResultType> Handle(AddBotCommand request, CancellationToken cancellationToken)
        {
            var owner = await _baseUserRepository.GetUserByIdAsync(request.ownerId);
            if (owner == null)
                return TransactionResultType.ForeignKeyNotFound;

            var newBot = new Bot
            {
                Name = request.bot.Name,
                Username = request.bot.Username,
                Description = request.bot.Description,
                Token = request.bot.Token
            };

            if (await _baseUserRepository.AddBotAsync(newBot, owner)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
