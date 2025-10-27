using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using MediatR;

namespace MASsenger.Application.Queries.BaseUserQueries
{
    public record GetUsersQuery() : IRequest<IEnumerable<User>>;
    public class GetUsersCommandHandler : IRequestHandler<GetUsersQuery, IEnumerable<User>>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public GetUsersCommandHandler(IBaseUserRepository baseUserRepository) 
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _baseUserRepository.GetUsersAsync();
        }
    }
}
