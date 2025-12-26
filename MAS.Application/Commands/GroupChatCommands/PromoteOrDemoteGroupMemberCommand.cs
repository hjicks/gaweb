using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Commands.GroupChatCommands;

public record PromoteOrDemoteGroupMemberCommand(int UserId, int GroupChatId, int MemberId) : IRequest<Result>;
public class PromoteOrDemoteGroupMemberCommandHandler : IRequestHandler<PromoteOrDemoteGroupMemberCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IBaseRepository<GroupChatUser> _groupChatUserRepository;
    private readonly IUnitOfWork _unitOfWork;
    public PromoteOrDemoteGroupMemberCommandHandler(IGroupChatRepository groupChatRepository,
        IBaseRepository<GroupChatUser> groupChatUserRepository, IUnitOfWork unitOfWork)
    {
        _groupChatRepository = groupChatRepository;
        _groupChatUserRepository = groupChatUserRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(PromoteOrDemoteGroupMemberCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId == request.MemberId)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (!groupChat.Members.Any(m => m.MemberId == request.UserId && m.Role == GroupChatRole.Owner))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var member = await _groupChatRepository.GetMemberAsync(request.GroupChatId, request.MemberId);
        if (member == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.MemberNotFound);

        if (member.IsBanned)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.MemberIsBanned);

        member.Role = member.Role == GroupChatRole.Member ? GroupChatRole.Admin : GroupChatRole.Member;

        _groupChatUserRepository.Update(member);
        await _unitOfWork.SaveAsync();

        Log.Information($"Owner {request.UserId} of group {groupChat.Id} changed role of member {member.MemberId}.");
        return Result.Success(StatusCodes.Status200OK, new GroupChatMemberGetDto
        {
            MemberId = member.MemberId,
            Role = member.Role,
            JoinedAt = member.JoinedAt,
            IsBanned = member.IsBanned
        });
    }
}
