using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Commands.SessionCommands
{
    /*
     * even though i named this as a command, this is actually a query.
     * as we have no sane way to renew the refresh token for now,
     * i named this a command so we can do that here as the last resort.
    */
    public record RefreshJwtCommand(Int32 SessionId, Guid RefreshToken) : IRequest<Result>;
    public class RefreshJwtCommandHandler : IRequestHandler<RefreshJwtCommand, Result>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        public RefreshJwtCommandHandler(ISessionRepository sessionRepository, IJwtService jwtService)
        {
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
        }
        public async Task<Result> Handle(RefreshJwtCommand request, CancellationToken cancellationToken)
        {
            var session = await _sessionRepository.GetByIdAsync(request.SessionId);
            if (session == null)
                return Result.Failure(StatusCodes.Status404NotFound, "Session not found.");

            if (session.Token != request.RefreshToken)
                return Result.Failure(StatusCodes.Status409Conflict, "FBI, open up!");

            if (session.ExpiresAt < DateTime.Now || session.IsExpired == true)
                return Result.Failure(StatusCodes.Status419AuthenticationTimeout,
                    "Session is expired, please login.");

            var roles = session.UserId == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
            return Result.Success(StatusCodes.Status200OK,
                new TokensResponse(_jwtService.GetJwt(session.UserId, roles)));
        }
    }
}
