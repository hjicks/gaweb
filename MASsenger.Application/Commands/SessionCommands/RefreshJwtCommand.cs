using MASsenger.Application.Interfaces;
using MediatR;
using System.Security.Claims;

namespace MASsenger.Application.Commands.SessionCommands
{
    /*
     * even though i named this as a command, this is actually a query.
     * as we have no sane way to renew the refresh token for now,
     * i named this a command so we can do that here as the last resort.
    */
    public record RefreshJwtCommand(Int32 userId, Guid refreshToken) : IRequest<string>;
    public class RefreshJwtCommandHandler : IRequestHandler<RefreshJwtCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        public RefreshJwtCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
        }
        public async Task<string> Handle(RefreshJwtCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);
            if (user == null) { return "Invalid User Id."; }
            var session = await _sessionRepository.GetActiveSessionByUserIdAsync(request.userId);
            if (session.Token != request.refreshToken) { return "Invalid Token"; }
            if (session.ExpiresAt < DateTimeOffset.Now) { return "Token is expired."; }
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            if (user.Username == "Admin") claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            claims.Add(new Claim(ClaimTypes.Role, "User"));
            return _jwtService.GetJwt(claims);
        }
    }
}
