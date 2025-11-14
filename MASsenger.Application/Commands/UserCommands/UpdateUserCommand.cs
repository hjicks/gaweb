using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record UpdateUserCommand(UserUpdateDto user) : IRequest<TransactionResultType>;
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, TransactionResultType>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<TransactionResultType> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(_userService.GetUsername());
            if (user == null)
                return TransactionResultType.ForeignKeyNotFound;
            using var hmac = new HMACSHA512();
            user.Name = request.user.Name;
            user.Username = request.user.Username;
            user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.user.Password));
            user.Description = request.user.Description;
            _userRepository.Update(user);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
