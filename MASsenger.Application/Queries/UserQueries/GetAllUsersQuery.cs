using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Interfaces;
using MediatR;

namespace MASsenger.Application.Queries.UserQueries
{
    public record GetAllUsersQuery() : IRequest<IEnumerable<UserReadDto>>;
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserReadDto>>
    {
        private readonly IUserRepository _userRepository;
        public GetAllUsersQueryHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<UserReadDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return (await _userRepository.GetAllAsync()).Select(u => new UserReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Description = u.Description,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified
            }).ToList(); ;
        }
    }
}
