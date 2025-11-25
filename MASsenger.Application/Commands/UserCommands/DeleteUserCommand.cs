using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MediatR;

namespace MASsenger.Application.Commands.UserCommands
{
    public record DeleteUserCommand(Int32 UserId) : IRequest<Result<BaseResponse>>;
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<BaseResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<BaseResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)   
                return new Result<BaseResponse>
                {
                    Success = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Description = "User not found."
                };

            _userRepository.Delete(user);
            await _unitOfWork.SaveAsync();

            return new Result<BaseResponse>
            {
                Success = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Description = "User deleted successfully."
            };
        }
    }
}
