using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Queries.GroupChatQueries;

public record GetGroupChatQuery(int UserId, int GroupChatId) : IRequest<Result>;
public class GetGroupChatQueryHandler : IRequestHandler<GetGroupChatQuery, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IBlobService _blobService;
    public GetGroupChatQueryHandler(IGroupChatRepository groupChatRepository, IBlobService blobService)
    {
        _groupChatRepository = groupChatRepository;
        _blobService = blobService;
    }
    public async Task<Result> Handle(GetGroupChatQuery request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);

        if (groupChat == null || groupChat.Members.Single().IsBanned == true)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        Log.Information($"Group chat {groupChat.Id} information fetched.");
        return Result.Success(StatusCodes.Status200OK, new GroupChatGetDto
        {
            Id = groupChat.Id,
            DisplayName = groupChat.DisplayName,
            Groupname = groupChat.Groupname!,
            Description = groupChat.Description,
            Avatar = _blobService.EncodeBlobToBase64(groupChat.Avatar!),
            IsPublic = groupChat.IsPublic,
            MsgPermissionType = groupChat.MsgPermissionType,
            CreatedAt = groupChat.CreatedAt
        });
    }
}
