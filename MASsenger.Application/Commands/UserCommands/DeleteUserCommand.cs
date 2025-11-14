using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.UserCommands
{
    public record DeleteUserCommand() : IRequest<TransactionResultType>;
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, TransactionResultType>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<TransactionResultType> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUsernameAsync(_userService.GetUsername());
            if (user == null)
                return TransactionResultType.ForeignKeyNotFound;
            _userRepository.Delete(user);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
