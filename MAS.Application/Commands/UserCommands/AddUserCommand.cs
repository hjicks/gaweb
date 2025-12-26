using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MAS.Core.Options;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace MAS.Application.Commands.UserCommands;

public record AddUserCommand(UserAddDto User) : IRequest<Result>;
public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly IJwtService _jwtService;
    private readonly IBlobService _blobService;
    private readonly TokenOptions _tokenOptions;
    public AddUserCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork, IHashService hashService, IJwtService jwtService,
        IBlobService blobService, IOptions<TokenOptions> tokenOptions)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtService = jwtService;
        _blobService = blobService;
        _tokenOptions = tokenOptions.Value;
    }
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.IsExistsAsync(request.User.Username);
        if (userExists)
            return Result.Failure(StatusCodes.Status422UnprocessableEntity, ErrorType.UsernameAlreadyExists);

        byte[] blob = null!;
        if (request.User.Avatar != null)
        {
            blob = _blobService.DecodeBase64Blob(request.User.Avatar);
            if (blob.IsNullOrEmpty())
                return Result.Failure(StatusCodes.Status422UnprocessableEntity, ErrorType.UnableToDecodeFileContent);
            if (!_blobService.ValidateImageBlob(blob))
                return Result.Failure(StatusCodes.Status422UnprocessableEntity, ErrorType.AvatarIsNotValid);
        }

        var passwordHash = _hashService.HashPassword(request.User.Password);
        var newUser = new User
        {
            DisplayName = request.User.DisplayName,
            Username = request.User.Username,
            PasswordHash = passwordHash.Hash,
            PasswordSalt = passwordHash.Salt,
            Bio = request.User.Bio,
            Avatar = blob
        };
        await _userRepository.AddAsync(newUser);

        var refreshTokenHash = _hashService.CreateAndHashRefreshToken();
        var session = new Session
        {
            TokenHash = refreshTokenHash.Hash,
            ExpiresAt = DateTime.UtcNow.AddDays(_tokenOptions.RefreshToken.ExpiryInDays),
            User = newUser,
            ClientName = request.User.ClientName,
            OS = request.User.OS
        };
        await _sessionRepository.AddAsync(session);

        await _unitOfWork.SaveAsync();

        var roles = newUser.Id == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };

        Log.Information($"User {newUser.Id} added.");
        return Result.Success(StatusCodes.Status201Created,
            new UserTokenDto
            {
                Id = newUser.Id,
                DisplayName = newUser.DisplayName,
                Username = newUser.Username,
                Bio = newUser.Bio,
                Avatar = request.User.Avatar,
                IsVerified = newUser.IsVerified,
                IsBot = newUser.IsBot,
                Jwt = _jwtService.GetJwt(newUser.Id, roles),
                RefreshToken = refreshTokenHash.RefreshToken
            });
    }
}
