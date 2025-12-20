using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MAS.Application.Commands.UserCommands;

public record UpdateUserCommand(int UserId, UserUpdateDto User) : IRequest<Result>;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                new[] { "User not found." });

        using var hmac = new HMACSHA512(user.PasswordSalt);
        user.DisplayName = request.User.DisplayName;
        user.Username = request.User.Username;
        user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.User.Password));
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
