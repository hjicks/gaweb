using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.GroupChatQueries;

public record GetGroupChatMembersQuery(int GroupChatId) : IRequest<Result>;
public class GetGroupChatMembersQueryHandler : IRequestHandler<GetGroupChatMembersQuery, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    public GetGroupChatMembersQueryHandler(IGroupChatRepository groupChatRepository)
    {
        _groupChatRepository = groupChatRepository;
    }
    public async Task<Result> Handle(GetGroupChatMembersQuery request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdWithMembersAsync(request.GroupChatId);

        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        var groupMembers = groupChat.Members.Select(m => new GroupChatMemberGetDto
        {
            MemberId = m.MemberId,
            Role = m.Role,
            IsBanned = m.IsBanned,
            JoinedAt = m.JoinedAt
        }).ToList();

        return Result.Success(StatusCodes.Status200OK, groupMembers);
    }
}
