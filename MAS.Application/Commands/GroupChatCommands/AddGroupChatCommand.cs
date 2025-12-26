using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Commands.GroupChatCommands;

public record AddGroupChatCommand(int OwnerId, GroupChatAddDto GroupChat) : IRequest<Result>;
public class AddGroupChatCommandHandler : IRequestHandler<AddGroupChatCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddGroupChatCommandHandler(IGroupChatRepository groupChatRepository, IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _groupChatRepository = groupChatRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddGroupChatCommand request, CancellationToken cancellationToken)
    {
        var owner = await _userRepository.GetByIdAsync(request.OwnerId);
        if (owner == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.UserNotFound);

        bool IsPublic = false;

        if (request.GroupChat.Groupname != null)
        {
            var groupExists = await _groupChatRepository.IsExistsAsync(request.GroupChat.Groupname);
            if (groupExists)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.GroupnameAlreadyExists);
            IsPublic = true;
        }

        var newGroupChat = new GroupChat
        {
            IsPublic = IsPublic,
            DisplayName = request.GroupChat.DisplayName,
            Groupname = IsPublic ? request.GroupChat.Groupname! : Guid.NewGuid().ToString(),
            Description = request.GroupChat.Description,
            Avatar = request.GroupChat.Avatar,
            MsgPermissionType = request.GroupChat.MsgPermissionType,
            Members = new List<GroupChatUser>
            {
                new() { Member = owner, Role = GroupChatRole.Owner }
            }
        };

        await _groupChatRepository.AddAsync(newGroupChat);
        await _unitOfWork.SaveAsync();

        Log.Information($"User {owner.Id} added group {newGroupChat.Id}.");
        return Result.Success(StatusCodes.Status200OK, new GroupChatGetDto
        {
            Id = newGroupChat.Id,
            DisplayName = newGroupChat.DisplayName,
            Groupname = newGroupChat.Groupname,
            Description = newGroupChat.Description,
            Avatar = newGroupChat.Avatar,
            IsPublic = newGroupChat.IsPublic,
            MsgPermissionType = newGroupChat.MsgPermissionType,
            CreatedAt = newGroupChat.CreatedAt
        });
    }
}
