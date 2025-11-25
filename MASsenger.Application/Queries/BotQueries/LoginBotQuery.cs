using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MediatR;

namespace MASsenger.Application.Queries.BotQueries
{
    public record LoginBotQuery(BotLoginDto Bot) : IRequest<string>;
    public class LoginBotQueryHandler : IRequestHandler<LoginBotQuery, string>
    {
        private readonly IBotRepository _botRepository;
        private readonly IJwtService _jwtService;
        public LoginBotQueryHandler(IBotRepository botRepository, IJwtService jwtService)
        {
            _botRepository = botRepository;
            _jwtService = jwtService;
        }
        public async Task<string> Handle(LoginBotQuery request, CancellationToken cancellationToken)
        {
            var dbBot = await _botRepository.GetByIdAsync(request.Bot.Id);
            if (dbBot == null) return "error";

            if (dbBot.Token.SequenceEqual(request.Bot.Token)) return "error";

            return _jwtService.GetJwt(dbBot.Id, new List<string> { "Bot" });
        }
    }
}
