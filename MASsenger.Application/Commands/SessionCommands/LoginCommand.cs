using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MASsenger.Core.Entities.UserEntities;
using MediatR;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.SessionCommands
{
    public record LoginCommand(UserLoginDto User) : IRequest<Result<TokensResponse>>;
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<TokensResponse>>
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
        public async Task<Result<TokensResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await _userRepository.GetByUsernameAsync(request.User.Username);
            if (dbUser == null)
                return new Result<TokensResponse>
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    Description = "Username or password is incorrect."
                };

            using var hmac = new HMACSHA512(dbUser.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.User.Password));
            if (!computedHash.SequenceEqual(dbUser.PasswordHash))
                return new Result<TokensResponse>
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.Conflict,
                    Description = "Username or password is incorrect."
                };

            var session = new Session
            {
                User = dbUser
            };
            await _sessionRepository.AddAsync(session);
            await _unitOfWork.SaveAsync();

            var roles = dbUser.Id == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
            return new Result<TokensResponse>
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Description = "Login successful.",
                Response = new TokensResponse(_jwtService.GetJwt(dbUser.Id, roles), session.Token)
            };
        }
    }
}
