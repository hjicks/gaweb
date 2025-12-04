using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Commands.PrivateChatCommands
{
    public record AddPrivateChatCommand(Int32 StarterId, Int32 ReceiverId) : IRequest<Result>;
    public class AddPrivateChatCommandHandler : IRequestHandler<AddPrivateChatCommand, Result>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddPrivateChatCommandHandler(IPrivateChatRepository privateChatRepository,IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _privateChatRepository = privateChatRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddPrivateChatCommand request, CancellationToken cancellationToken)
        {
            var starter = await _userRepository.GetByIdAsync(request.StarterId);
            var receiver = await _userRepository.GetByIdAsync(request.ReceiverId);

            if (receiver == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "Receiver not found." });
            
            var newPrivateChat = new PrivateChat
            {
                Starter = starter!,
                Receiver = receiver
            };

            await _privateChatRepository.AddAsync(newPrivateChat);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status200OK,
                new BaseResponse("Chat added successfully."));
        }
    }
}
