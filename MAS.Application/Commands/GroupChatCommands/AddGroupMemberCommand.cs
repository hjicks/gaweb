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
        if (request.UserId == request.MemberId)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (!groupChat.Members.Any(m => m.MemberId == request.UserId))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var user = await _userRepository.GetByIdAsync(request.MemberId);
        if (user == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        var memberExists = await _groupChatRepository.IsMemberExistsAsync(request.GroupChatId, request.MemberId);
        if (memberExists)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.MemberAlreadyJoinedOrIsBanned);

        var groupChatUser = new GroupChatUser()
        {
            GroupChat = groupChat,
            Member = user
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
