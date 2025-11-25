using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MASsenger.Core.Entities.UserEntities;
using MediatR;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record LoginUserCommand(UserLoginDto user) : IRequest<Result<TokensResponse>>;
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<TokensResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        public LoginUserCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }
        public async Task<Result<TokensResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User dbUser = await _userRepository.GetByUsernameAsync(request.user.Username);
            if (dbUser == null) return new Result<TokensResponse>
            {
                Success = false,
                Response = new TokensResponse
                {
                    Message = "Username or password is incorrect."
                },
                StatusCode = System.Net.HttpStatusCode.Unauthorized
            };

            using var hmac = new HMACSHA512(dbUser.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.user.Password));
            if (!computedHash.SequenceEqual(dbUser.PasswordHash)) return new Result<TokensResponse>
            {
                Success = false,
                Response = new TokensResponse
                {
                    Message = "Username or password is incorrect."
                },
                StatusCode = System.Net.HttpStatusCode.Unauthorized
            };

            var session = new Session
            {
                User = dbUser
            };
            await _sessionRepository.AddAsync(session);

            await _unitOfWork.SaveAsync();

            return new Result<TokensResponse>
            {
                Success = true,
                Response = new TokensResponse
                {
                    Jwt = _jwtService.GetJwt(dbUser.Id, dbUser.Id == 1 ? "Admin" : "User"),
                    RefreshToken = session.Token.ToString(),
                    Message = "Login successful."
                },
                StatusCode = System.Net.HttpStatusCode.OK
            };
        }
    }
}
