using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Constants;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Commands.GroupChatCommands;

public record LeaveGroupChatCommand(int UserId, int GroupChatId) : IRequest<Result>;
public class LeaveGroupChatCommandHandler : IRequestHandler<LeaveGroupChatCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUnitOfWork _unitOfWork;
    public LeaveGroupChatCommandHandler(IGroupChatRepository groupChatRepository, IUnitOfWork unitOfWork)
    {
        _groupChatRepository = groupChatRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(LeaveGroupChatCommand request, CancellationToken cancellationToken)
    {
        var groupChat = await _groupChatRepository.GetByIdWithMemberAsync(request.UserId, request.GroupChatId);
        if (groupChat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (!groupChat.Members.Any(m => m.MemberId == request.UserId && m.IsBanned == false))
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

        var member = groupChat.Members.Single();
        groupChat.Members.Remove(member);

        _groupChatRepository.Update(groupChat);
        await _unitOfWork.SaveAsync();

        Log.Information($"User {member.MemberId} left group {groupChat.Id}.");
        return Result.Success(StatusCodes.Status200OK,
            ResponseMessages.Success[SuccessType.LeaveSuccessful]);
    }
}
