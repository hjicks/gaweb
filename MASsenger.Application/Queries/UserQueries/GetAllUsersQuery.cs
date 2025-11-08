using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MediatR;

namespace MASsenger.Application.Queries.UserQueries
{
    public record GetAllUsersQuery() : IRequest<IEnumerable<User>>;
    public class GetUsersCommandHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        private readonly IUserRepository _userRepository;
        public GetUsersCommandHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync();
        }
    }
}
