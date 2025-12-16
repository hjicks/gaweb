using MAS.Application.Dtos.PrivateChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.PrivateChatQueries
{
    public record GetAllUserPrivateChatsQuery(Int32 UserId) : IRequest<Result>;
    public class GetAllUserPrivateChatsQueryHandler : IRequestHandler<GetAllUserPrivateChatsQuery, Result>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        public GetAllUserPrivateChatsQueryHandler(IPrivateChatRepository privateChatRepository)
        {
            _privateChatRepository = privateChatRepository;
        }
        public async Task<Result> Handle(GetAllUserPrivateChatsQuery request, CancellationToken cancellationToken)
        {
            var chats = (await _privateChatRepository.GetAllUserAsync(request.UserId)).Select(c => new PrivateChatGetDto
            {
                Id = c.Id,
                Members = c.Members,
                CreatedAt = c.CreatedAt
            }).ToList();

            return Result.Success(StatusCodes.Status200OK, chats);
        }
    }
}
