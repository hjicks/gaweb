using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities.UserEntities;
using MediatR;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record AddUserCommand(UserCreateDto user) : IRequest<(string, string)>;
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, (string, string)>
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService; 
        public AddUserCommandHandler(IUserRepository userRepository, ISessionRepository sessionRepository, IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }
        public async Task<(string, string)> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            using var hmac = new HMACSHA512();
            var newUser = new User
            {
                Name = request.user.Name,
                Username = request.user.Username,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.user.Password)),
                PasswordSalt = hmac.Key,
                Description = request.user.Description
            };
            await _userRepository.AddAsync(newUser);

            var session = new Session
            {
                User = newUser
            };
            await _sessionRepository.AddAsync(session);
           
            await _unitOfWork.SaveAsync();

            return (_jwtService.GetJwt(newUser.Id, newUser.Id == 1 ? "Admin" : "User"),
                session.Token.ToString());
        }
    }
}
