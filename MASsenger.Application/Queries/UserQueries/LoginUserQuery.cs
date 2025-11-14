using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MediatR;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MASsenger.Application.Queries.UserQueries
{
    public record LoginUserQuery(UserLoginDto user) : IRequest<string>;
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public LoginUserQueryHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<string> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            User dbUser = await _userRepository.GetByUsernameAsync(request.user.Username);
            if (dbUser == null) return "error";

            using var hmac = new HMACSHA512(dbUser.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.user.Password));
            if (!computedHash.SequenceEqual(dbUser.PasswordHash)) return "error";

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, dbUser.Username),
                new Claim(ClaimTypes.Role, "User")
            };
            return _jwtService.GetJwt(claims);
        }
    }
}
