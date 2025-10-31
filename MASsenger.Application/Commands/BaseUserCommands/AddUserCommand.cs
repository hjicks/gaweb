using MASsenger.Core.Dto.Create;
using MASsenger.Core.Entities;
using MASsenger.Core.Enums;
using MASsenger.Core.Interfaces;
using MediatR;

namespace MASsenger.Application.Commands.BaseUserCommands
{
    public record AddUserCommand(UserCreateDto user) : IRequest<TransactionResultType>;
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, TransactionResultType>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public AddUserCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }
        public async Task<TransactionResultType> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                Name = request.user.Name,
                Username = request.user.Username,
                Description = request.user.Description
            };
            if (await _baseUserRepository.AddUserAsync(newUser)) return TransactionResultType.Done;
            return TransactionResultType.SaveChangesError;
        }
    }
}
