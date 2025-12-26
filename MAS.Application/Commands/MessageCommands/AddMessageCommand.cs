using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace MAS.Application.Commands.MessageCommands;

public record AddMessageCommand(int SenderId, MessageAddDto Message) : IRequest<Result>;
public class AddMessageCommandHandler : IRequestHandler<AddMessageCommand, Result>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBaseChatRepository _baseChatRepository;
    private readonly IPrivateChatRepository _privateChatRepository;
    private readonly IGroupChatRepository _groupChatRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBlobService _blobService;
    private readonly IHubContext<ChatHub> _hubContext;

    public AddMessageCommandHandler(IMessageRepository messageRepository,
        IUserRepository userRepository, IBaseChatRepository baseChatRepository,
        IPrivateChatRepository privateChatRepository, IGroupChatRepository groupChatRepository,
        IUnitOfWork unitOfWork, IBlobService blobService, IHubContext<ChatHub> hubContext)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _baseChatRepository = baseChatRepository;
        _privateChatRepository = privateChatRepository;
        _groupChatRepository = groupChatRepository;
        _unitOfWork = unitOfWork;
        _blobService = blobService;
        _hubContext = hubContext;
    }
    public async Task<Result> Handle(AddMessageCommand request, CancellationToken cancellationToken)
    {
        var sender = await _userRepository.GetByIdAsync(request.SenderId);

        var destination = await _baseChatRepository.GetByIdAsync(request.Message.DestinationId);
        if (destination == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.ChatNotFound);

        if (destination.Type == ChatType.Group)
        {
            var group = await _groupChatRepository.GetByIdWithMembersAsync(destination.Id);
            var groupMember = group!.Members.Where(m => m.MemberId == request.SenderId).SingleOrDefault();

            if (groupMember == null)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

            if (groupMember!.IsBanned == true)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);

            if (group!.MsgPermissionType == GroupMsgPermissionType.OnlyAdmins &&
                (groupMember!.Role != GroupChatRole.Owner && groupMember.Role != GroupChatRole.Admin))
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);
        }

        if (destination.Type == ChatType.Private)
        {
            var pv = await _privateChatRepository.GetByIdWithMembersAsync(destination.Id);
            var pvMember = pv!.Members.Where(m => m.Id == request.SenderId).SingleOrDefault();

            if (pvMember == null)
                return Result.Failure(StatusCodes.Status409Conflict, ErrorType.PermissionDenied);
        }

        byte[] blob = null!;
        if (request.Message.Content != null)
        {
            blob = _blobService.DecodeBase64Blob(request.Message.Content);
            if (blob.IsNullOrEmpty())
                return Result.Failure(StatusCodes.Status422UnprocessableEntity, ErrorType.UnableToDecodeFileContent);
        }

        var newMessage = new Message
        {
            Sender = sender!,
            DestinationId = destination.Id,
            Text = request.Message.Text
        };
        if (request.Message.Content != null)
        {
            newMessage.FileName = request.Message.FileName;
            newMessage.FileSize = request.Message.FileSize;
            newMessage.FileContentType = request.Message.FileContentType;
            newMessage.FileContent = new FileContent { Content = blob };
        }

        await _messageRepository.AddAsync(newMessage);
        await _unitOfWork.SaveAsync();

        var msg = new MessageGetDto
        {
            Id = newMessage.Id,
            SenderId = newMessage.SenderId,
            DestinationId = newMessage.DestinationId,
            Text = newMessage.Text,
            FileName = newMessage.FileName,
            FileSize = newMessage.FileSize,
            FileContentType = newMessage.FileContentType,
            CreatedAt = newMessage.CreatedAt
        };

        if (_baseChatRepository.GetTypeByIdAsync(request.Message.DestinationId).Result == (int)ChatType.Private)
        {
            PrivateChat pc = await _privateChatRepository.GetByIdAsync(destination.Id);
            int dstUserId = sender!.Id == pc.Members.First<User>().Id ? pc.Members.Last<User>().Id : pc.Members.First<User>().Id;
            User u = await _userRepository.GetByIdAsync(dstUserId);
            await _hubContext.Clients.User(u.Id.ToString()).SendAsync("AddMessage",
                msg, cancellationToken: cancellationToken);
        }
        else if (_baseChatRepository.GetTypeByIdAsync(request.Message.DestinationId).Result == (int)ChatType.Group)
        {
            GroupChat gc = await _groupChatRepository.GetByIdAsync(destination.Id);
            foreach(GroupChatUser gcu in gc.Members)
            {
                /* you don't want to send that message to sender, do you? */
                if (gcu.MemberId == sender!.Id)
                    continue;

                await _hubContext.Clients.User(gcu.MemberId.ToString()).SendAsync("AddMessage",
                    msg, cancellationToken: cancellationToken);
            }
        }

        Log.Information($"User {sender!.Id} added message {newMessage.Id} to chat {destination.Id}.");
        return Result.Success(StatusCodes.Status201Created, msg);
    }
}
