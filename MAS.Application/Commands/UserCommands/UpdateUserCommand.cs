using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.UserCommands;

public record UpdateUserCommand(int UserId, UserUpdateDto User) : IRequest<Result>;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork,
        IHashService hashService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
    }
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        var passwordHash = _hashService.HashPassword(request.User.Password);
        user.DisplayName = request.User.DisplayName;
        user.Username = request.User.Username;
        user.PasswordHash = passwordHash.Hash;
        user.PasswordSalt = passwordHash.Salt;
        user.Bio = request.User.Bio;
        user.Avatar = request.User.Avatar;
        _userRepository.Update(user);
        await _unitOfWork.SaveAsync();

        return Result.Success(StatusCodes.Status200OK,
            new UserGetDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Username = user.Username,
                Bio = user.Bio,
                Avatar = user.Avatar,
                IsVerified = user.IsVerified,
                IsBot = user.IsBot,
                LastSeenAt = user.LastSeenAt
            });
    }
}
