using MAS.Application.Dtos.BotDtos;
using MAS.Application.Interfaces;
using MediatR;

namespace MAS.Application.Queries.BotQueries
{
    public record GetAllBotsQuery() : IRequest<IEnumerable<BotReadDto>>;
    public class GetAllBotsQueryHandler : IRequestHandler<GetAllBotsQuery, IEnumerable<BotReadDto>>
    {
        private readonly IBotRepository _botRepository;
        public GetAllBotsQueryHandler(IBotRepository botRepository)
        {
            _botRepository = botRepository;
        }
        public async Task<IEnumerable<BotReadDto>> Handle(GetAllBotsQuery request, CancellationToken cancellationToken)
        {
            return (await _botRepository.GetAllAsync()).Select(u => new BotReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Description = u.Description,
                Token = u.Token,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified,
                IsActive = u.IsActive
            }).ToList();
        }
    }
}
