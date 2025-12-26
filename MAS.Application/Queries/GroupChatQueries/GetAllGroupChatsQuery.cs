using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.GroupChatQueries;

public record GetAllGroupChatsQuery() : IRequest<Result>;
public class GetAllGroupChatsQueryHandler : IRequestHandler<GetAllGroupChatsQuery, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IBlobService _blobService;
    public GetAllGroupChatsQueryHandler(IGroupChatRepository groupChatRepository, IBlobService blobService)
    {
        _groupChatRepository = groupChatRepository;
        _blobService = blobService;
    }
    public async Task<Result> Handle(GetAllGroupChatsQuery request, CancellationToken cancellationToken)
    {
        var groupChats = (await _groupChatRepository.GetAllAsync()).Select(g => new GroupChatGetDto
        {
            Id = g.Id,
            DisplayName = g.DisplayName,
            Groupname = g.Groupname!,
            Description = g.Description,
            Avatar = _blobService.EncodeBlobToBase64(g.Avatar!),
            IsPublic = g.IsPublic,
            MsgPermissionType = g.MsgPermissionType,
            CreatedAt = g.CreatedAt
        }).ToList();

        return Result.Success(StatusCodes.Status200OK, groupChats); 
    }
}
