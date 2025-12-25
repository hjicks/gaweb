using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.GroupChatCommands;

public record JoinGroupChatCommand(int UserId, int GroupChatId) : IRequest<Result>;
public class JoinGroupChatCommandHandler : IRequestHandler<JoinGroupChatCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public JoinGroupChatCommandHandler(IGroupChatRepository groupChatRepository, IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _groupChatRepository = groupChatRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(JoinGroupChatCommand request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (groupChat.Members.Any(m => m.MemberId == request.UserId))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.MemberAlreadyJoinedOrIsBanned);

        if (!groupChat.IsPublic)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        var groupChatUser = new GroupChatUser()
        {
            GroupChat = groupChat,
            Member = user,
        };
        groupChat.Members.Add(groupChatUser);

        _groupChatRepository.Update(groupChat);
        await _unitOfWork.SaveAsync();

        return Result.Success(StatusCodes.Status200OK, new GroupChatMemberGetDto
        {
            MemberId = groupChatUser.MemberId,
            Role = groupChatUser.Role,
            JoinedAt = groupChatUser.JoinedAt,
            IsBanned = groupChatUser.IsBanned
        });
    }
}
