using MASsenger.Core.Dto.Create;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MASsenger.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MASsenger.Application.Commands
{
    public record AddBotCommand(BotCreateDto bot, UInt64 ownerId) : IRequest<TransactionResultType>;
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
