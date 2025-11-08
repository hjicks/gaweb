using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MediatR;

namespace MASsenger.Application.Queries.BotQueries
{
    public record GetAllBotsQuery() : IRequest<IEnumerable<Bot>>;
    public class GetBotsCommandHandler : IRequestHandler<GetAllBotsQuery, IEnumerable<Bot>>
    {
        private readonly IBotRepository _botRepository;
        public GetBotsCommandHandler(IBotRepository botRepository)
        {
            _botRepository = botRepository;
        }
        public async Task<IEnumerable<Bot>> Handle(GetAllBotsQuery request, CancellationToken cancellationToken)
        {
            return await _botRepository.GetAllAsync();
        }
    }
}
