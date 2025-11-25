using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MASsenger.Core.Entities.UserEntities;
using MediatR;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record AddUserCommand(UserCreateDto User) : IRequest<Result<TokensResponse>>;
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Result<TokensResponse>>
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
        public async Task<Result<TokensResponse>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var dbUser = await _userRepository.GetByUsernameAsync(request.User.Username);
            if (dbUser != null)
                return new Result<TokensResponse>
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
                    Description = "Username is already taken. Please choose another."
                };

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
                User = newUser
            };
            await _sessionRepository.AddAsync(session);
           
            await _unitOfWork.SaveAsync();

            var roles = newUser.Id == 1 ? new List<string> { "Admin", "User" } : new List<string> { "User" };
            return new Result<TokensResponse>
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Response = new TokensResponse(_jwtService.GetJwt(newUser.Id, roles), session.Token)
            };
        }
    }
}
