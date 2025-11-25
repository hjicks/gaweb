using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MediatR;

namespace MASsenger.Application.Commands.SessionCommands
{
    /*
     * even though i named this as a command, this is actually a query.
     * as we have no sane way to renew the refresh token for now,
     * i named this a command so we can do that here as the last resort.
    */
    public record RefreshJwtCommand(Int32 sessionId, Guid refreshToken) : IRequest<Result<TokensResponse>>;
    public class RefreshJwtCommandHandler : IRequestHandler<RefreshJwtCommand, Result<TokensResponse>>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IJwtService _jwtService;
        public RefreshJwtCommandHandler(ISessionRepository sessionRepository, IJwtService jwtService)
        {
            _sessionRepository = sessionRepository;
            _jwtService = jwtService;
        }
        public async Task<Result<TokensResponse>> Handle(RefreshJwtCommand request, CancellationToken cancellationToken)
        {
            var session = await _sessionRepository.GetByIdAsync(request.sessionId);
            if (session == null)
            {
                return new Result<TokensResponse> 
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Response = new TokensResponse
                    {
                        Message = "Session not found."
                    }
                }; 
            }
            if (session.Token != request.refreshToken)
            {
                return new Result<TokensResponse>
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.Unauthorized,
                    Response = new TokensResponse
                    {
                        Message = "FBI, open up!"
                    }
                };
            }
            if (session.ExpiresAt < DateTime.Now)
            {
                return new Result<TokensResponse>
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.Forbidden,
                    Response = new TokensResponse
                    {
                        Message = "Session is expired, please login."
                    }
                };
            }
            
            return new Result<TokensResponse>
            {
                Success = true,
                StatusCode= System.Net.HttpStatusCode.OK,
                Response = new TokensResponse
                {
                    Jwt = _jwtService.GetJwt(session.UserId, session.UserId == 1 ? "Admin" : "User"),
                }
            };
        }
    }
}
