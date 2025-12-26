using MAS.Application.Dtos.PrivateChatDtos;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

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
        var privateChats = (await _privateChatRepository.GetAllUserAsync(request.UserId)).Select(p => new PrivateChatGetDto
        {
            Id = p.Id,
            Receiver = new UserGetDto
            {
                Id = p.Members.Single().Id,
                DisplayName = p.Members.Single().DisplayName,
                Username = p.Members.Single().Username!,
                Bio = p.Members.Single().Bio,
                Avatar = p.Members.Single().Avatar,
                IsVerified = p.Members.Single().IsVerified,
                IsBot = p.Members.Single().IsBot,
                LastSeenAt = p.Members.Single().LastSeenAt
            },
            CreatedAt = p.CreatedAt
        }).ToList();

        Log.Information($"All private chats of user {request.UserId} fetched.");
        return Result.Success(StatusCodes.Status200OK, privateChats);
    }
}
