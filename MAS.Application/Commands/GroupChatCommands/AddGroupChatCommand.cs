using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.GroupChatCommands;

public record AddGroupChatCommand(int OwnerId, PublicGroupChatAddDto GroupChat) : IRequest<Result>;
public class AddChannelChatCommandHandler : IRequestHandler<AddGroupChatCommand, Result>
{
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    public AddChannelChatCommandHandler(IGroupChatRepository groupChatRepository, IUserRepository userRepository,
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

        var newPublicGroupChat = new GroupChat
        {
            DisplayName = request.GroupChat.DisplayName,
            Groupname = request.GroupChat.Groupname,
            Description = request.GroupChat.Description,
            Avatar = request.GroupChat.Avatar,
            MsgPermissionType = request.GroupChat.MsgPermissionType,
            IsPublic = true,
            Members = new List<GroupChatUser>
            {
                new() { Member = owner, Role = GroupChatRole.Owner }
            }
        };

        await _groupChatRepository.AddAsync(newPublicGroupChat);
        await _unitOfWork.SaveAsync();
        
        return Result.Success(StatusCodes.Status200OK, new PublicGroupChatAddDto
        {
            DisplayName = newPublicGroupChat.DisplayName,
            Groupname = newPublicGroupChat.Groupname,
            Description = newPublicGroupChat.Description,
            Avatar = newPublicGroupChat.Avatar,
            MsgPermissionType = newPublicGroupChat.MsgPermissionType
        });
    }
}
