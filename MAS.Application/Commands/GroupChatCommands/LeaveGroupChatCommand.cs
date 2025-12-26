using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace MAS.Application.Commands.GroupChatCommands;

public record LeaveGroupChatCommand(int UserId, int GroupChatId) : IRequest<Result>;
public class LeaveGroupChatCommandHandler : IRequestHandler<LeaveGroupChatCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<ChatHub> _hubContext;
    public LeaveGroupChatCommandHandler(IGroupChatRepository groupChatRepository, IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
    {
        _groupChatRepository = groupChatRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }
    public async Task<Result> Handle(LeaveGroupChatCommand request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (!groupChat.Members.Any(m => m.MemberId == request.UserId && m.IsBanned == false))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var member = groupChat.Members.Single();
        groupChat.Members.Remove(member);

        _groupChatRepository.Update(groupChat);
        await _unitOfWork.SaveAsync();

        /*
         * Assuming everything went well so far,
         * Inevitability we had to fetch list of group members somewhere, sooner or later
         * (note the *s* on end of function's name)
         */
        groupChat = await _groupChatRepository.GetByIdWithMembersAsync(request.GroupChatId);
        foreach (GroupChatUser gcu in groupChat.Members)
        {
            await _hubContext.Clients.User(gcu.MemberId.ToString()).SendAsync("LeaveGroupChat",
                request.GroupChatId, request.UserId, cancellationToken: cancellationToken);
        }

        Log.Information($"User {member.MemberId} left group {groupChat.Id}.");
        return Result.Success(StatusCodes.Status200OK, "Member removed from the group successfully.");
    }
}
