using MAS.Application.Dtos.PrivateChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.PrivateChatQueries;

public record GetAllPrivateChatsQuery() : IRequest<Result>;
public class GetAllPrivateChatsQueryHandler : IRequestHandler<GetAllPrivateChatsQuery, Result>
{
    private readonly IPrivateChatRepository _privateChatRepository;
    public GetAllPrivateChatsQueryHandler(IPrivateChatRepository privateChatRepository)
    {
        _privateChatRepository = privateChatRepository;
    }
    public async Task<Result> Handle(GetAllPrivateChatsQuery request, CancellationToken cancellationToken)
    {
        var privateChats = (await _privateChatRepository.GetAllAsync()).Select(c => new PrivateChatGetDto
        {
            Id = c.Id,
            Receiver = null!,
            CreatedAt = c.CreatedAt
        }).ToList();

        return Result.Success(StatusCodes.Status200OK, privateChats);
    }
}
