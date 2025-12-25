using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.GroupChatCommands;

public record AddGroupMemberCommand(int UserId, int GroupChatId, int MemberId) : IRequest<Result>;
public class AddGroupMemberCommandHandler : IRequestHandler<AddGroupMemberCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddGroupMemberCommandHandler(IGroupChatRepository groupChatRepository, IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _groupChatRepository = groupChatRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(AddGroupMemberCommand request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdWithMembersAsync(request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (!groupChat.Members.Any(m => m.MemberId == request.UserId))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var member = await _userRepository.GetByIdAsync(request.MemberId);
        if (member == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        if (groupChat.Members.Any(m => m.MemberId == request.MemberId))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.GroupMemberAlreadyJoinedOrIsBanned);

        var groupChatUser = new GroupChatUser()
        {
            GroupChatId = groupChat.Id,
            MemberId = member.Id
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
