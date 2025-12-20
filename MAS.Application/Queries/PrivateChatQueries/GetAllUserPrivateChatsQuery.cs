using MAS.Application.Dtos.PrivateChatDtos;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.PrivateChatQueries;

public record GetAllUserPrivateChatsQuery(int UserId) : IRequest<Result>;
public class GetAllUserPrivateChatsQueryHandler : IRequestHandler<GetAllUserPrivateChatsQuery, Result>
{
    private readonly IPrivateChatRepository _privateChatRepository;
    public GetAllUserPrivateChatsQueryHandler(IPrivateChatRepository privateChatRepository)
    {
        _privateChatRepository = privateChatRepository;
    }
    public async Task<Result> Handle(GetAllUserPrivateChatsQuery request, CancellationToken cancellationToken)
    {
        var privateChats = (await _privateChatRepository.GetAllUserAsync(request.UserId)).Select(c => new PrivateChatGetDto
        {
            Id = c.Id,
            Receiver = new UserGetDto
            {
                Id = c.Members.Single().Id,
                DisplayName = c.Members.Single().DisplayName,
                Username = c.Members.Single().Username,
                Bio = c.Members.Single().Bio,
                Avatar = c.Members.Single().Avatar,
                IsVerified = c.Members.Single().IsVerified,
                IsBot = c.Members.Single().IsBot,
                LastSeenAt = c.Members.Single().LastSeenAt
            },
            CreatedAt = c.CreatedAt
        }).ToList();

        return Result.Success(StatusCodes.Status200OK, privateChats);
    }
}
