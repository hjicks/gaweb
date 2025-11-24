using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record UpdateUserCommand(Int32 userId, UserUpdateDto user) : IRequest<TransactionResultType>;
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, TransactionResultType>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.userId);
            if (user == null)
                return TransactionResultType.ForeignKeyNotFound;
            using var hmac = new HMACSHA512(user.PasswordSalt);
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
