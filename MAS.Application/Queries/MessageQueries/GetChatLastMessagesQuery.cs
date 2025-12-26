using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MAS.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace MAS.Application.Queries.MessageQueries;

public record GetChatLastMessagesQuery(int MessageId) : IRequest<Result>;
public class GetChatLastMessagesQueryHandler : IRequestHandler<GetChatLastMessagesQuery, Result>
{
    private readonly IMessageRepository _messageRepository;
    public GetChatLastMessagesQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public async Task<Result> Handle(GetChatLastMessagesQuery request, CancellationToken cancellationToken)
    {
        var message = await _messageRepository.GetByIdAsync(request.MessageId);
        if (message == null)
            return Result.Failure(StatusCodes.Status404NotFound, ErrorType.MessageNotFound);

        ushort msgCount = 20;
        var messages = (await _messageRepository.GetChatLastMessagesAsync(message.DestinationId, message.CreatedAt, msgCount))
            .Select(m => new MessageGetDto
            {
                Id = m.Id,
                SenderId = m.SenderId,
                DestinationId = m.DestinationId,
                Text = m.Text,
                FileName = m.FileName,
                FileSize = m.FileSize,
                FileContentType = m.FileContentType,
                CreatedAt = m.CreatedAt
            }).ToList(); ;

        Log.Information($"Last {msgCount} messages before message {message.Id} fetched from chat {message.DestinationId}.");
        return Result.Success(StatusCodes.Status200OK, messages);
    }
}

