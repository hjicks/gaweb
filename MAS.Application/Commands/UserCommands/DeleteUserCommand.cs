using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Constants;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Commands.UserCommands;

public record DeleteUserCommand(int UserId) : IRequest<Result>;
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPrivateChatRepository _privateChatRepository;
    private readonly IBaseRepository<PrivateChatUser> _privateChatUserRepository;
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IBaseRepository<GroupChatUser> _groupChatUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteUserCommandHandler(IUserRepository userRepository,
        IPrivateChatRepository privateChatRepository, IBaseRepository<PrivateChatUser> privateChatUserRepository,
        IGroupChatRepository groupChatRepository, IBaseRepository<GroupChatUser> groupChatUserRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _privateChatRepository = privateChatRepository;
        _privateChatUserRepository = privateChatUserRepository;
        _groupChatRepository = groupChatRepository;
        _groupChatUserRepository = groupChatUserRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        user.IsDeleted = true;
        user.DisplayName = "Deleted account";
        user.Username = null;
        user.Bio = null;
        user.Avatar = null;
        user.UpdatedAt = DateTime.UtcNow;
        _userRepository.Update(user);

        var userPrivateChats = await _privateChatRepository.GetAllUserMembershipsAsync(request.UserId);
        _privateChatUserRepository.DeleteRange(userPrivateChats);

        var userGroupChats = await _groupChatRepository.GetAllUserMembershipsAsync(request.UserId);
        _groupChatUserRepository.DeleteRange(userGroupChats);

        await _unitOfWork.SaveAsync();

        Log.Information($"User {user.Id} deleted.");
        return Result.Success(StatusCodes.Status200OK,
            ResponseMessages.Success[SuccessType.DeleteSuccessful]);
    }
}
