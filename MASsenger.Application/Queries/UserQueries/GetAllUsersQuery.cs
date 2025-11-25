using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MediatR;

namespace MASsenger.Application.Queries.UserQueries
{
    public record GetAllUsersQuery() : IRequest<Result<GetEntityResponse<UserReadDto>>>;
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<GetEntityResponse<UserReadDto>>>
    {
        private readonly IUserRepository _userRepository;
        public GetAllUsersQueryHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<Result<GetEntityResponse<UserReadDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            return new Result<GetEntityResponse<UserReadDto>>
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Response = new GetEntityResponse<UserReadDto>(
                    (await _userRepository.GetAllAsync()).Select(u => new UserReadDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Username = u.Username,
                        Description = u.Description,
                        CreatedAt = u.CreatedAt,
                        IsVerified = u.IsVerified
                    }).ToList())
            };
        }
    }
}
