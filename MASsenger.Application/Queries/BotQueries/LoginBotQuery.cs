using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MediatR;
using System.Security.Claims;

namespace MASsenger.Application.Queries.BotQueries
{
    public record LoginBotQuery(BotLoginDto bot) : IRequest<string>;
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
            Bot dbBot = await _botRepository.GetByIdAsync(request.bot.Id);
            if (dbBot == null) return "error";

            if (dbBot.Token.SequenceEqual(request.bot.Token)) return "error";

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, dbBot.Id.ToString()),
            };
            claims.Add(new Claim(ClaimTypes.Role, "Bot"));
            return _jwtService.GetJwt(claims);
        }
    }
}
