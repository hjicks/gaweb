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
        private readonly IRepository<Bot> _botRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddBotCommandHandler(IRepository<Bot> botRepository, IRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _botRepository = botRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(AddBotCommand request, CancellationToken cancellationToken)
        {
            var owner = await _userRepository.GetByIdAsync(request.ownerId);
            if (owner == null)
                return TransactionResultType.ForeignKeyNotFound;

            var newBot = new Bot
            {
                Name = request.bot.Name,
                Username = request.bot.Username,
                Description = request.bot.Description,
                Token = request.bot.Token
            };

            newBot.Owner = owner;
            _botRepository.Add(newBot);
            await _unitOfWork.SaveAsync();
            if (true) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
