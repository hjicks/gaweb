using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Queries.UserQueries
{
    public record GetAllUsersQuery() : IRequest<Result>;
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result>
    {
        private readonly IUserRepository _userRepository;
        public GetAllUsersQueryHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<Result> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = (await _userRepository.GetAllAsync()).Select(u => new UserReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Description = u.Description,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified
            }).ToList();

            return Result.Success(StatusCodes.Status200OK, users);
        }
    }
}
