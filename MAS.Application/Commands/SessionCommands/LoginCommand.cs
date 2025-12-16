using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MAS.Application.Commands.SessionCommands
{
    public record LoginCommand(UserLoginDto User) : IRequest<Result>;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        public LoginCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository,
            IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }
        public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await _userRepository.GetByUsernameAsync(request.User.Username);
            if (dbUser == null)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.InvalidCredentials,
                    new[] { "Username or password is incorrect." });

            using var hmac = new HMACSHA512(dbUser.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.User.Password));
            if (!computedHash.SequenceEqual(dbUser.PasswordHash))
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.InvalidCredentials,
                    new[] { "Username or password is incorrect." });

            var session = new Session
            {
                UserId = dbUser.Id
            };
            await _sessionRepository.AddAsync(session);
            await _unitOfWork.SaveAsync();

            var roles = dbUser.Id == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
            return Result.Success(StatusCodes.Status200OK,
                new SessionRefreshDto
                {
                    Jwt = _jwtService.GetJwt(dbUser.Id, roles),
                    RefreshToken = session.Token.ToString()
                });
        }
    }
}
