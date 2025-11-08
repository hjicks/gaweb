using MASsenger.Application.Dto.Update;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.UserCommands
{
    public record UpdateUserCommand(UserUpdateDto user) : IRequest<TransactionResultType>;
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
            var user = await _userRepository.GetByIdAsync(request.user.Id);
            if (user == null)
                return TransactionResultType.ForeignKeyNotFound;
            user.Name = request.user.Name;
            user.Username = request.user.Username;
            user.Description = request.user.Description;
            _userRepository.Update(user);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
