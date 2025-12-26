using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace MAS.Application.Commands.GroupChatCommands;

public record JoinGroupChatCommand(int UserId, int GroupChatId) : IRequest<Result>;
public class JoinGroupChatCommandHandler : IRequestHandler<JoinGroupChatCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemMsgService _systemMsgService;
    private readonly IHubContext<ChatHub> _hubContext;
  
    public JoinGroupChatCommandHandler(IGroupChatRepository groupChatRepository, IUserRepository userRepository,
        IUnitOfWork unitOfWork, ISystemMsgService systemMsgService, IHubContext<ChatHub> hubContext)
    {
        _groupChatRepository = groupChatRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _systemMsgService = systemMsgService;
        _hubContext = hubContext;
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

        GroupChatMemberGetDto msg = new()
        {
            MemberId = groupChatUser.MemberId,
            Role = groupChatUser.Role,
            JoinedAt = groupChatUser.JoinedAt,
            IsBanned = groupChatUser.IsBanned
        };

        /*
         * Assuming everything went well so far,
         * Inevitability we had to fetch list of group members somewhere, sooner or later
         * (note the *s* on end of function's name)
         */
        groupChat = await _groupChatRepository.GetByIdWithMembersAsync(request.GroupChatId);
        foreach (GroupChatUser gcu in groupChat.Members)
        {
            /* you don't want to send that message to sender, do you? */
        if (gcu.MemberId == user!.Id)
                continue;

            await _hubContext.Clients.User(gcu.MemberId.ToString()).SendAsync("JoinGroupChat",
                msg, groupChat.Id, cancellationToken: cancellationToken);
        }

        await _systemMsgService.SendSystemMsgAsync(groupChat.Id, MasEvent.Join, user.Id!);
        Log.Information($"User {user.Id} joined group {groupChat.Id}.");
        return Result.Success(StatusCodes.Status200OK, msg);
    }
}
