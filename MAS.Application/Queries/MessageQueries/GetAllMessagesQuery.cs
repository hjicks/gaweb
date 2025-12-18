using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Interfaces;
using MAS.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MAS.Application.Queries.MessageQueries;

public record GetAllMessagesQuery() : IRequest<Result>;
public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, Result>
{
    private readonly IMessageRepository _messageRepository;
    public GetAllMessagesQueryHandler(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public async Task<Result> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
    {
        var messages = (await _messageRepository.GetAllAsync()).Select(m => new MessageGetDto
        {
            Id = m.Id,
            SenderId = m.SenderId,
            DestinationId = m.DestinationId,
            Text = m.Text,
            FileName = m.FileName,
            FileSize = m.FileSize,
            FileContentType = m.FileContentType,
            CreatedAt = m.CreatedAt
        }).ToList();

        return Result.Success(StatusCodes.Status200OK, messages);
    }
}
