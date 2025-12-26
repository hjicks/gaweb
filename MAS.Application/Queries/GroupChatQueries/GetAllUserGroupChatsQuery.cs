using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Queries.GroupChatQueries;

public record GetAllUserGroupChatsQuery(int UserId) : IRequest<Result>;
public class GetAllUserGroupChatsQueryHandler : IRequestHandler<GetAllUserGroupChatsQuery, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IBlobService _blobService;
    public GetAllUserGroupChatsQueryHandler(IGroupChatRepository groupChatRepository, IBlobService blobService)
    {
        _groupChatRepository = groupChatRepository;
        _blobService = blobService;
    }
    public async Task<Result> Handle(GetAllUserGroupChatsQuery request, CancellationToken cancellationToken)
    {
        var groupChats = (await _groupChatRepository.GetAllUserAsync(request.UserId)).Select(g => new GroupChatGetDto
        {
            Id = g.Id,
            IsPublic = g.IsPublic,
            DisplayName = g.DisplayName,
            Groupname = g.Groupname!,
            Description = g.Description,
            Avatar = _blobService.EncodeBlobToBase64(g.Avatar!),
            MsgPermissionType = g.MsgPermissionType,
            CreatedAt = g.CreatedAt
        }).ToList();

        Log.Information($"All group chats of user {request.UserId} fetched.");
        return Result.Success(StatusCodes.Status200OK, groupChats);
    }
}
