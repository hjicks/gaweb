using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Commands.SessionCommands;

public record RefreshSessionCommand(SessionRefreshTokenDto TokenDto) : IRequest<Result>;
public class RefreshSessionCommandHandler : IRequestHandler<RefreshSessionCommand, Result>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly IJwtService _jwtService;

    public RefreshSessionCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork,
        IHashService hashService, IJwtService jwtService)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtService = jwtService;
    }
    public async Task<Result> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByTokenAsync(
            _hashService.HashRefreshToken(request.TokenDto.RefreshToken));
        if (session == null)
        {
            Log.Warning($"Unsuccessful attempt to refresh a session, token used: {request.TokenDto.RefreshToken}");
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.SessionNotFound);
        }
            

        if (session.ExpiresAt < DateTime.Now || session.IsRevoked == true)
            return Result.Failure(StatusCodes.Status419AuthenticationTimeout, ErrorType.InvalidOrExpiredRefreshToken);

        var tokenHash = _hashService.CreateAndHashRefreshToken();
        session.TokenHash = tokenHash.Hash;
        session.ExpiresAt = DateTime.UtcNow.AddDays(7);
        session.UpdatedAt = DateTime.UtcNow;
        _sessionRepository.Update(session);
        await _unitOfWork.SaveAsync();

        var roles = session.UserId == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };

        Log.Information($"Jwt of session {session.Id} renewed.");
        return Result.Success(StatusCodes.Status200OK,
            new SessionRefreshDto
            {
                Jwt = _jwtService.GetJwt(session.UserId, roles),
                RefreshToken = tokenHash.RefreshToken
            });
    }
}
