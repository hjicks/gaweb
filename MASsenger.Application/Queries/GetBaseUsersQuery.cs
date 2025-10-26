using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using MediatR;

namespace MASsenger.Application.Queries
{
    public record GetBaseUsersQuery() : IRequest<IEnumerable<BaseUser>>;
    public class GetBaseUsersHandler : IRequestHandler<GetBaseUsersQuery, IEnumerable<BaseUser>>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public GetBaseUsersHandler(IBaseUserRepository baseUserRepository) 
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<IEnumerable<BaseUser>> Handle(GetBaseUsersQuery request, CancellationToken cancellationToken)
        {
            return await _baseUserRepository.GetBaseUsersAsync();
        }
    }
}
