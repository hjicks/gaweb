using MASsenger.Application.Dtos.BotDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.BotCommands
{
    public record AddBotCommand(BotCreateDto bot, Int32 ownerId) : IRequest<TransactionResultType>;
    public class AddBotCommandHandler : IRequestHandler<AddBotCommand, TransactionResultType>
    {
        private readonly IBotRepository _botRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddBotCommandHandler(IBotRepository botRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
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
            await _botRepository.AddAsync(newBot);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
