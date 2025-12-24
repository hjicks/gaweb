using MAS.Application.Dtos.PrivateChatDtos;
using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace MAS.Application.Commands.PrivateChatCommands;

public record AddPrivateChatCommand(int StarterId, int ReceiverId) : IRequest<Result>;
public class AddPrivateChatCommandHandler : IRequestHandler<AddPrivateChatCommand, Result>
{
    private readonly IPrivateChatRepository _privateChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<ChatHub> _hubContext;

    public AddPrivateChatCommandHandler(IPrivateChatRepository privateChatRepository,IUserRepository userRepository, IUnitOfWork unitOfWork, IHubContext<ChatHub> hubContext)
    {
        _privateChatRepository = privateChatRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hubContext = hubContext;
    }

    public async Task<Result> Handle(AddPrivateChatCommand request, CancellationToken cancellationToken)
    {
        if (request.StarterId == request.ReceiverId)
        {
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);
        }

        var starter = await _userRepository.GetByIdWithPrivateChatsAsync(request.StarterId);
        var receiver = await _userRepository.GetByIdWithPrivateChatsAsync(request.ReceiverId);

        if (receiver == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        var starterPvChatsId = starter!.PrivateChats.Select(p => p.Id).ToList();
        var receiverPvChatsId = receiver.PrivateChats.Select(p => p.Id).ToList();

        foreach (var starterPvChatId in starterPvChatsId)
        {
            foreach (var receiverPvChatId in receiverPvChatsId)
            {
                if (starterPvChatId == receiverPvChatId)
                    return Result.Failure(StatusCodes.Status409Conflict, ErrorType.ChatAlreadyExists);
            }
        }

        var newPrivateChat = new PrivateChat
        {
            Members = new List<User> { starter!, receiver }
        };

        await _privateChatRepository.AddAsync(newPrivateChat);
        await _unitOfWork.SaveAsync();

        PrivateChatGetDto pc = new PrivateChatGetDto
        {
            Id = newPrivateChat.Id,
            Receiver = null!,
            CreatedAt = newPrivateChat.CreatedAt
        };

        await _hubContext.Clients.User(receiver.Id.ToString()).SendAsync("AddPrivateChatCommand", pc, cancellationToken: cancellationToken);

        return Result.Success(StatusCodes.Status201Created, pc);
    }
}
