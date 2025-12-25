using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace MAS.Application.Commands.GroupChatCommands;

public record DeleteGroupChatCommand(int UserId, int GroupChatId) : IRequest<Result>;
public class DeleteGroupChatCommandHandler : IRequestHandler<DeleteGroupChatCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteGroupChatCommandHandler(IGroupChatRepository groupChatRepository, IUnitOfWork unitOfWork)
    {
        _groupChatRepository = groupChatRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteGroupChatCommand request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (groupChat.Members.IsNullOrEmpty() || groupChat.Members.Single().Role != GroupChatRole.Owner)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        groupChat.IsDeleted = true;
        groupChat.DisplayName = "Deleted group";
        groupChat.Groupname = null;
        groupChat.Description = null;
        groupChat.Avatar = null;

        _groupChatRepository.Update(groupChat);
        await _unitOfWork.SaveAsync();

        return Result.Success(StatusCodes.Status200OK, "Chat deleted successfully.");
    }
}
