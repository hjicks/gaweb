using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.UserCommands;

public record UpdateUserLastSeenCommand(int UserId, UserLastSeenUpdateDto User) : IRequest<Result>;
public class UpdateUserLastSeenCommandHandler : IRequestHandler<UpdateUserLastSeenCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateUserLastSeenCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(UpdateUserLastSeenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        user.LastSeenAt = request.User.LastSeenAt;

        _userRepository.Update(user);
        await _unitOfWork.SaveAsync();

        return Result.Success(StatusCodes.Status200OK,
            new UserLastSeenUpdateDto
            {
                LastSeenAt = user.LastSeenAt
            });
    }
}
