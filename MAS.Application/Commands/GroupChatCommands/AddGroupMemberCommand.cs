using System.Reflection;
using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Application.Queries.GroupChatQueries;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace MAS.Application.Commands.GroupChatCommands;

public record AddGroupMemberCommand(int UserId, int GroupChatId, int MemberId) : IRequest<Result>;
public class AddGroupMemberCommandHandler : IRequestHandler<AddGroupMemberCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<ChatHub> _hubContext;
    public AddGroupMemberCommandHandler(IGroupChatRepository groupChatRepository, IUserRepository userRepository,
        IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
    {
        _groupChatRepository = groupChatRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;

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

        var member = await _userRepository.GetByIdAsync(request.MemberId);
        if (member == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        var memberExists = await _groupChatRepository.IsMemberExistsAsync(request.GroupChatId, request.MemberId);
        if (memberExists)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.MemberAlreadyJoinedOrIsBanned);

        var groupChatUser = new GroupChatUser()
        {
            GroupChat = groupChat,
            Member = member
        };
        groupChat.Members.Add(groupChatUser);

        _groupChatRepository.Update(groupChat);
        await _unitOfWork.SaveAsync();

        GroupChatMemberGetDto msg = new()
        {
            MemberId = groupChatUser.MemberId,
            Role = groupChatUser.Role,
            JoinedAt = groupChatUser.JoinedAt,
            IsBanned = groupChatUser.IsBanned
        };

        /*
         * TODO:
         * we can't use GroupChatMemberGetDto here, since the person who got added right now has no idea which group he added to
         * it's not the best solution we have, but we are running short of time.
         */
        await _hubContext.Clients.User(request.MemberId.ToString()).SendAsync("AddGroupMemberCommand",
                request.GroupChatId, cancellationToken: cancellationToken);
        /*
        foreach (GroupChatUser gcu in groupChat.Members)
        {
            // TODO: should we send a message again?
            if (gcu.MemberId == request.UserId) continue;
            await _hubContext.Clients.User(gcu.MemberId.ToString()).SendAsync("AddGroupMemberCommand", msg);
        }
        */
        Log.Information($"User {request.UserId} added member {member.Id} to group {groupChat.Id}.");

        return Result.Success(StatusCodes.Status200OK, msg);
    }
}
