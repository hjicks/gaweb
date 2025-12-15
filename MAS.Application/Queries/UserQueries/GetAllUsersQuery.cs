using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.UserQueries
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
            var users = (await _userRepository.GetAllAsync()).Select(u => new UserGetDto
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Username = u.Username,
                Bio = u.Bio,
                IsVerified = u.IsVerified,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();

            return Result.Success(StatusCodes.Status200OK, users);
        }
    }
}
