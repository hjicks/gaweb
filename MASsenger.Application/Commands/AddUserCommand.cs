using MASsenger.Core.Dto;
using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using MediatR;

namespace MASsenger.Application.Commands
{
    public record AddUserCommand(UserDto user) : IRequest<bool>;
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public AddUserCommandHandler(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }
        public Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new User
            {
                Name = request.user.Name,
                Username = request.user.Username,
                Description = request.user.Description
            };
            return _baseUserRepository.AddUserAsync(newUser);
        }
    }
}
