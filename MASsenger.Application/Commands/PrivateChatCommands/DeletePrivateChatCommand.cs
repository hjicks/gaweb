using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MASsenger.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MASsenger.Application.Commands.PrivateChatCommands
{
    public record DeletePrivateChatCommand(int PrivateChatId) : IRequest<Result>;
    public class DeletePrivateChatCommandHandler : IRequestHandler<DeletePrivateChatCommand, Result>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DeletePrivateChatCommandHandler(IPrivateChatRepository privateChatRepository, IUnitOfWork unitOfWork)
        {
            _privateChatRepository = privateChatRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeletePrivateChatCommand request, CancellationToken cancellationToken)
        {
            var privateChat = await _privateChatRepository.GetByIdAsync(request.PrivateChatId);
            if (privateChat == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                    new[] { "User not found." });

            _privateChatRepository.Delete(privateChat);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status204NoContent);
        }
    }
}
