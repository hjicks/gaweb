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
            var chats = (await _privateChatRepository.GetAllUserAsync(request.UserId)).Select(c => new PrivateChatReadDto
            {
                Id = c.Id,
                StarterId = c.StarterId,
                ReceiverId = c.ReceiverId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return Result.Success(StatusCodes.Status200OK, chats);
        }
    }
}
