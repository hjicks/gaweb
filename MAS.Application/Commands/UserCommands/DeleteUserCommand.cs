using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.UserCommands
{
    public record DeleteUserCommand(int UserId) : IRequest<Result>;
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "User not found." });

            _userRepository.Delete(user);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status204NoContent);
        }
    }
}
