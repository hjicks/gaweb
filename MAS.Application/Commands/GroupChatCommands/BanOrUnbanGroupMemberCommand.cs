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

public record BanOrUnbanGroupMemberCommand(int UserId, int GroupChatId, int MemberId) : IRequest<Result>;
public class BanOrUnbanGroupMemberCommandHandler : IRequestHandler<BanOrUnbanGroupMemberCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IBaseRepository<GroupChatUser> _groupChatUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISystemMsgService _systemMsgService;
    private readonly IHubContext<ChatHub> _hubContext;
    public BanOrUnbanGroupMemberCommandHandler(IGroupChatRepository groupChatRepository,
        IBaseRepository<GroupChatUser> groupChatUserRepository, IUnitOfWork unitOfWork, 
        ISystemMsgService systemMsgService, IHubContext<ChatHub> hubContext)
    {
        _groupChatRepository = groupChatRepository;
        _groupChatUserRepository = groupChatUserRepository;
        _unitOfWork = unitOfWork;
        _systemMsgService = systemMsgService;
        _hubContext = hubContext;
    }
    public async Task<Result> Handle(BanOrUnbanGroupMemberCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == request.MemberId)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (!groupChat.Members.Any(m => m.MemberId == request.UserId && m.Role == GroupChatRole.Owner || m.Role == GroupChatRole.Admin))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var member = await _groupChatRepository.GetMemberAsync(request.GroupChatId, request.MemberId);
        if (member == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.MemberNotFound);

        member.IsBanned = !member.IsBanned;

        _groupChatUserRepository.Update(member);
        await _unitOfWork.SaveAsync();

        /* TODO: do we need more information to provide to the client? */
        await _hubContext.Clients.User(request.MemberId.ToString()).SendAsync("BanOrUnbanGroupMemberCommand",
           request.GroupChatId, member.IsBanned, cancellationToken: cancellationToken);

        if (member.IsBanned)
            await _systemMsgService.SendSystemMsgAsync(groupChat.Id, MasEvent.Ban, member.MemberId);
        Log.Information($"Admin {request.UserId} of group {groupChat.Id} {(member.IsBanned ? "banned" : "unbanned")} member {member.MemberId}.");
        return Result.Success(StatusCodes.Status200OK, new GroupChatMemberGetDto
        {
            MemberId = member.MemberId,
            Role = member.Role,
            JoinedAt = member.JoinedAt,
            IsBanned = member.IsBanned
        });
    }
}
