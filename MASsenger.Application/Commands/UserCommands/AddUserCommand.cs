using MASsenger.Application.Dtos.UserDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record AddUserCommand(UserCreateDto User) : IRequest<Result>;
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService; 
        public AddUserCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository,
            IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }
        public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await _userRepository.GetByUsernameAsync(request.User.Username);
            if (dbUser != null)
                return Result.Failure(StatusCodes.Status422UnprocessableEntity, ErrorType.AlreadyExists,
                    new[] { "Username is already taken. Please choose another." });

            using var hmac = new HMACSHA512();
            var newUser = new User
            {
                Name = request.User.Name,
                Username = request.User.Username,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.User.Password)),
                PasswordSalt = hmac.Key,
                Description = request.User.Description
            };
            await _userRepository.AddAsync(newUser);

            var session = new Session
            {
                UserId = newUser.Id
            };
            await _sessionRepository.AddAsync(session);
           
            await _unitOfWork.SaveAsync();

            var roles = newUser.Id == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
            return Result.Success(StatusCodes.Status201Created,
                new UserTokenDto
                {
                    Id = newUser.Id,
                    Name = newUser.Name,
                    Username = newUser.Username,
                    Description = newUser.Description,
                    IsVerified = newUser.IsVerified,
                    CreatedAt = newUser.CreatedAt,
                    UpdatedAt = newUser.UpdatedAt,
                    Jwt = _jwtService.GetJwt(newUser.Id, roles),
                    RefreshToken = session.Token.ToString()
                });
        }
    }
}
