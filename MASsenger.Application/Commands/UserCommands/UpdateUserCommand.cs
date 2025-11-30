using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

namespace MASsenger.Application.Commands.UserCommands
{
    public record UpdateUserCommand(Int32 UserId, UserUpdateDto User) : IRequest<Result<BaseResponse>>;
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<BaseResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<BaseResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                return new Result<BaseResponse>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Description = "User not found."
                };

            using var hmac = new HMACSHA512(user.PasswordSalt);
            user.Name = request.User.Name;
            user.Username = request.User.Username;
            user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.User.Password));
            user.Description = request.User.Description;
            _userRepository.Update(user);
            await _unitOfWork.SaveAsync();

            return new Result<BaseResponse>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Description = "User updated successfully."
            };
        }
    }
}
