using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Enums;
using MediatR;

namespace MASsenger.Application.Commands.UserCommands
{
    public record UpdateUserPasswdCommand(UserPasswdUpdateDto user) : IRequest<TransactionResultType>;
    public class UpdateUserPasswdCommandHandler : IRequestHandler<UpdateUserPasswdCommand, TransactionResultType>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserPasswdCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<TransactionResultType> Handle(UpdateUserPasswdCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.user.Id);
            if (user == null)
                return TransactionResultType.ForeignKeyNotFound;
            user.Passwd = request.user.Passwd;
            _userRepository.Update(user);
            await _unitOfWork.SaveAsync();
            return TransactionResultType.Done;
        }
    }
}
