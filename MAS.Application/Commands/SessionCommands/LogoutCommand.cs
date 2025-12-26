using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Constants;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Commands.SessionCommands;

public record LogoutCommand(SessionRefreshTokenDto TokenDto) : IRequest<Result>;
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    public LogoutCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork,
        IHashService hashService)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
    }
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var dbSession = await _sessionRepository.GetByTokenAsync(
            _hashService.HashRefreshToken(request.TokenDto.RefreshToken));
        if (dbSession == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.SessionNotFound);

        dbSession.IsRevoked = true;
        dbSession.RevokedAt = DateTime.UtcNow;
        dbSession.IsDeleted = true;

        _sessionRepository.Update(dbSession);
        await _unitOfWork.SaveAsync();

        Log.Information($"Session with id {dbSession.Id} is revoked.");
        return Result.Success(StatusCodes.Status200OK,
            ResponseMessages.Success[SuccessType.LogoutSuccessful]);
    }
}
