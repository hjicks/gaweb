using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MAS.Application.Commands.SessionCommands;

public record RefreshSessionCommand(string RefreshToken) : IRequest<Result>;
public class RefreshSessionCommandHandler : IRequestHandler<RefreshSessionCommand, Result>
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;

    public RefreshSessionCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }
    public async Task<Result> Handle(RefreshSessionCommand request, CancellationToken cancellationToken)
    {
        var session = await _sessionRepository.GetByTokenAsync(request.RefreshToken);
        if (session == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.SessionNotFound);

        if (session.Token != request.RefreshToken)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.InvalidOrExpiredRefreshToken);

        if (session.ExpiresAt < DateTime.Now || session.IsRevoked == true)
            return Result.Failure(StatusCodes.Status419AuthenticationTimeout, ErrorType.InvalidOrExpiredRefreshToken);

        session.Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        session.ExpiresAt = DateTime.UtcNow.AddDays(7);
        session.UpdatedAt = DateTime.UtcNow;
        _sessionRepository.Update(session);
        await _unitOfWork.SaveAsync();

        var roles = session.UserId == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
        return Result.Success(StatusCodes.Status200OK,
            new SessionRefreshDto
            {
                Jwt = _jwtService.GetJwt(session.UserId, roles),
                RefreshToken = session.Token.ToString()
            });
    }
}
