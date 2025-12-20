using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.GroupChatQueries;

public record GetGroupChatQuery(int GroupChatId) : IRequest<Result>;
public class GetGroupChatQueryHandler : IRequestHandler<GetGroupChatQuery, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    public GetGroupChatQueryHandler(IGroupChatRepository groupChatRepository)
    {
        _groupChatRepository = groupChatRepository;
    }
    public async Task<Result> Handle(GetGroupChatQuery request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdAsync(request.GroupChatId);

        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        return Result.Success(StatusCodes.Status200OK, new GroupChatGetDto
        {
            Id = groupChat.Id,
            DisplayName = groupChat.DisplayName,
            Groupname = groupChat.DisplayName,
            Description = groupChat.DisplayName,
            Avatar = groupChat.Avatar,
            IsPublic = groupChat.IsPublic,
            MsgPermissionType = groupChat.MsgPermissionType,
            CreatedAt = groupChat.CreatedAt
        });
    }
}
