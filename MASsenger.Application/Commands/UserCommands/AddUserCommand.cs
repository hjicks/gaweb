using MASsenger.Application.Dto.Create;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MediatR;

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
            var newUser = new User
            {
                Name = request.user.Name,
                Username = request.user.Username,
                Description = request.user.Description
            };
            _userRepository.Add(newUser);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
