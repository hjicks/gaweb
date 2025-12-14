using MAS.Application.Dtos.PrivateChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.PrivateChatCommands
{
    public record AddPrivateChatCommand(int StarterId, int ReceiverId) : IRequest<Result>;
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
                StarterId = starter!.Id,
                ReceiverId = receiver.Id
            };

            await _privateChatRepository.AddAsync(newPrivateChat);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status201Created,
                new PrivateChatReadDto
                {
                    Id = newPrivateChat.Id,
                    StarterId = newPrivateChat.StarterId,
                    ReceiverId = newPrivateChat.ReceiverId,
                    CreatedAt = newPrivateChat.CreatedAt,
                    UpdatedAt = newPrivateChat.UpdatedAt
                });
        }
    }
}
