using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace MASsenger.Application.Queries.MessageQueries
{
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
            var messages = (await _messageRepository.GetAllAsync()).Select(m => new MessageReadDto
            {
                Text = m.Text,
                CreatedAt = m.CreatedAt,
                SenderID = m.Sender.Id,
                DestinationID = m.Destination.Id,
            }).ToList();

            return Result.Success(StatusCodes.Status200OK, messages);
        }
    }
}
