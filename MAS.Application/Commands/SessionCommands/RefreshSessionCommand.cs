using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.SessionCommands
{
    /*
     * even though i named this as a command, this is actually a query.
     * as we have no sane way to renew the refresh token for now,
     * i named this a command so we can do that here as the last resort.
    */
    public record RefreshSessionCommand(Int32 SessionId, Guid RefreshToken) : IRequest<Result>;
    public class RefreshJwtCommandHandler : IRequestHandler<RefreshSessionCommand, Result>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        public RefreshJwtCommandHandler(ISessionRepository sessionRepository, IJwtService jwtService)
        {
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
        }
        public async Task<Result> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await _sessionRepository.GetByIdAsync(request.SessionId);
            if (session == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "Session not found." });

            if (session.Token != request.RefreshToken)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.InvalidRefreshToken,
                    new[] { "FBI, open up!" });

            if (session.ExpiresAt < DateTime.Now || session.IsRevoked == true)
                return Result.Failure(StatusCodes.Status419AuthenticationTimeout, ErrorType.ExpiredRefreshToken,
                    new[] { "Session is expired, please login." });

            var roles = session.UserId == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
            return Result.Success(StatusCodes.Status200OK,
                new SessionRefreshDto
                {
                    Jwt = _jwtService.GetJwt(session.UserId, roles)
                });
        }
    }
}
