using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MediatR;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record AddUserCommand(UserCreateDto user) : IRequest<TransactionResultType>;
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, TransactionResultType>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(AddUserCommand request, CancellationToken cancellationToken)
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
            _userRepository.Add(newUser);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
