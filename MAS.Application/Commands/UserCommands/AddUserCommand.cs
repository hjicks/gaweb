using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MAS.Application.Commands.UserCommands;

public record AddUserCommand(UserAddDto User) : IRequest<Result>;
public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService; 
    public AddUserCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository,
        IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var userExists = await _userRepository.IsExistsAsync(request.User.Username);
        if (userExists)
            return Result.Failure(StatusCodes.Status422UnprocessableEntity, ErrorType.UsernameAlreadyExists);

        using var hmac = new HMACSHA512();
        var newUser = new User
        {
            DisplayName = request.User.DisplayName,
            Username = request.User.Username,
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.User.Password)),
            PasswordSalt = hmac.Key,
            Bio = request.User.Bio,
            Avatar = request.User.Avatar
        };
        await _userRepository.AddAsync(newUser);

        var session = new Session
        {
            User = newUser,
            ClientName = request.User.ClientName,
            OS = request.User.OS
        };
        await _sessionRepository.AddAsync(session);

        await _unitOfWork.SaveAsync();

        var roles = newUser.Id == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
        return Result.Success(StatusCodes.Status201Created,
            new UserTokenDto
            {
                Id = newUser.Id,
                DisplayName = newUser.DisplayName,
                Username = newUser.Username,
                Bio = newUser.Bio,
                Avatar = newUser.Avatar,
                IsVerified = newUser.IsVerified,
                IsBot = newUser.IsBot,
                Jwt = _jwtService.GetJwt(newUser.Id, roles),
                RefreshToken = session.Token.ToString()
            });
    }
}
