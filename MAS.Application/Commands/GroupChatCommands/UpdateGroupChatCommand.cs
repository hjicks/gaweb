using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Commands.GroupChatCommands
{
    public record UpdateGroupChatCommand(GroupChatUpdateDto GroupChat) : IRequest<Result>;
    public class UpdateGroupChatCommandHandler : IRequestHandler<UpdateGroupChatCommand, Result>
    {
        private readonly IGroupChatRepository _groupChatRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UpdateGroupChatCommandHandler(IGroupChatRepository groupChatRepository, IUnitOfWork unitOfWork)
        {
            _groupChatRepository = groupChatRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateGroupChatCommand request, CancellationToken cancellationToken)
        {
            var groupChat = await _groupChatRepository.GetByIdAsync(request.GroupChat.Id);
            if (groupChat == null)
                return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

            bool IsPublic = false;

            if (request.GroupChat.Groupname != null)
            {
                IsPublic = true;
            }

            groupChat.IsPublic = IsPublic;
            groupChat.DisplayName = request.GroupChat.DisplayName;
            groupChat.Groupname = IsPublic ? request.GroupChat.Groupname! : Guid.NewGuid().ToString();
            groupChat.Description = request.GroupChat.Description;
            groupChat.Avatar = request.GroupChat.Avatar;
            groupChat.MsgPermissionType = request.GroupChat.MsgPermissionType;

            _groupChatRepository.Update(groupChat);
            await _unitOfWork.SaveAsync();

            return Result.Success(StatusCodes.Status200OK, new GroupChatGetDto
            {
                Id = groupChat.Id,
                DisplayName = groupChat.DisplayName,
                Groupname = groupChat.Groupname,
                Description = groupChat.Description,
                Avatar = groupChat.Avatar,
                IsPublic = groupChat.IsPublic,
                MsgPermissionType = groupChat.MsgPermissionType,
                CreatedAt = groupChat.CreatedAt
            });
        }
    }
}
