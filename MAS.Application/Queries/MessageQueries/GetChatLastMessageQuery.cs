using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Entities.ChatEntities;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.MessageQueries;

public record GetChatLastMessageQuery(int ChatId) : IRequest<Result>;
public class GetChatLastMessageQueryHandler : IRequestHandler<GetChatLastMessageQuery, Result>
{
    private readonly IMessageRepository _messageRepository;
    private readonly IBaseRepository<BaseChat> _baseChatRepository;
    public GetChatLastMessageQueryHandler(IMessageRepository messageRepository, IBaseRepository<BaseChat> baseRepository)
    {
        _messageRepository = messageRepository;
        _baseChatRepository = baseRepository;
    }
    public async Task<Result> Handle(GetChatLastMessageQuery request, CancellationToken cancellationToken)
    {
        var chat = await _baseChatRepository.GetByIdAsync(request.ChatId);
        if (chat == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.NotFound,
                new[] { "Chat not found." });

        var message = await _messageRepository.GetChatLastMessageAsync(request.ChatId);
        if (message == null)
            return Result.Failure(StatusCodes.Status409Conflict, ErrorType.ChatIsEmpty,
                new[] { "Chat have no message." });

        return Result.Success(StatusCodes.Status200OK, new MessageGetDto
        {
            Id = message.Id,
            SenderId = message.SenderId,
            DestinationId = message.DestinationId,
            Text = message.Text,
            FileName = message.FileName,
            FileSize = message.FileSize,
            FileContentType = message.FileContentType,
            CreatedAt = message.CreatedAt
        });
    }
}
