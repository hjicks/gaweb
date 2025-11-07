using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MediatR;

namespace MASsenger.Application.Queries.BaseUserQueries
{
    public record GetAllBotsQuery() : IRequest<IEnumerable<Bot>>;
    public class GetBotsCommandHandler : IRequestHandler<GetAllBotsQuery, IEnumerable<Bot>>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public GetBotsCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<IEnumerable<Bot>> Handle(GetAllBotsQuery request, CancellationToken cancellationToken)
        {
            return await _baseUserRepository.GetAllBotsAsync();
        }
    }
}
