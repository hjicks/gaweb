using MASsenger.Core.Enums;
using MASsenger.Core.Interfaces;
using MediatR;

namespace MASsenger.Application.Commands
{
    public record DeleteUserCommand(UInt64 userId) : IRequest<TransactionResultType>;
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, TransactionResultType>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public DeleteUserCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }

        public async Task<TransactionResultType> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _baseUserRepository.GetUserByIdAsync(request.userId);
            if (user == null)
                return TransactionResultType.ForeignKeyNotFound;
            if (await _baseUserRepository.DeleteUserAsync(user)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
