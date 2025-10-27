using MASsenger.Core.Dto;
using MASsenger.Core.Enums;
using MASsenger.Core.Interfaces;
using MediatR;

namespace MASsenger.Application.Commands
{
    public record UpdateUserCommand(UserUpdateDto user) : IRequest<TransactionResultType>;
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, TransactionResultType>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public UpdateUserCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<TransactionResultType> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _baseUserRepository.GetUserByIdAsync(request.user.Id);
            if (user == null)
                return TransactionResultType.ForeignKeyNotFound;
            user.Name = request.user.Name;
            user.Username = request.user.Username;
            user.Description = request.user.Description;
            if (await _baseUserRepository.UpdateUserAsync(user)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
