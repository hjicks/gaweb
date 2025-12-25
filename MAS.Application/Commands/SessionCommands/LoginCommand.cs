using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Application.Services;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.SessionCommands;

public record LoginCommand(UserLoginDto User) : IRequest<Result>;
public class LoginCommandHandler : IRequestHandler<LoginCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly IJwtService _jwtService;
    public LoginCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork, IHashService hashService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtService = jwtService;
    }
    public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.GetByUsernameAsync(request.User.Username);
        if (dbUser == null)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.InvalidCredentials);

        var passwordHash = new PasswordHash(dbUser.PasswordHash, dbUser.PasswordSalt);
        var isPasswordCorrect = _hashService.VerifyPassword(request.User.Password, passwordHash);
        if (!isPasswordCorrect)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.InvalidCredentials);

#if !DEBUG
    /* seems this is really troublesome if the client crashes..., let's disable it for now */
        var hasActiveSession = await _sessionRepository.GetActiveAsync(dbUser.Id);
        if (hasActiveSession == true)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.ActiveSessionAvailable);
#endif
        var refreshTokenHash = _hashService.CreateAndHashRefreshToken();
        var session = new Session
        {
            TokenHash = refreshTokenHash.Hash,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            User = dbUser,
            ClientName = request.User.ClientName,
            OS = request.User.OS
        };
        await _sessionRepository.AddAsync(session);
        await _unitOfWork.SaveAsync();

        var roles = dbUser.Id == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
        return Result.Success(StatusCodes.Status200OK,
            new SessionRefreshDto
            {
                Jwt = _jwtService.GetJwt(dbUser.Id, roles),
                RefreshToken = refreshTokenHash.RefreshToken
            });
    }
}
