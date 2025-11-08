using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MediatR;

namespace MASsenger.Application.Queries.BaseUserQueries
{
    public record GetAllUsersQuery() : IRequest<IEnumerable<User>>;
    public class GetUsersCommandHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public GetUsersCommandHandler(IBaseUserRepository baseUserRepository) 
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _baseUserRepository.GetAllUsersAsync();
        }
    }
}
