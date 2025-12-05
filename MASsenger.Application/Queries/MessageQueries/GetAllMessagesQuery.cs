using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Interfaces;
using MediatR;

namespace MASsenger.Application.Queries.MessageQueries
{
    public record GetAllMessagesQuery() : IRequest<IEnumerable<MessageReadDto>>;
    public class GetAllMessagesQueryHandler : IRequestHandler<GetAllMessagesQuery, IEnumerable<MessageReadDto>>
    {
        private readonly IMessageRepository _messageRepository;
        public GetAllMessagesQueryHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<IEnumerable<MessageReadDto>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
        {
            return (await _messageRepository.GetAllAsync()).Select(m => new MessageReadDto
            {
                Text = m.Text,
                CreatedAt = m.CreatedAt,
                SenderID = m.Sender.Id,
                DestinationID = m.Destination.Id,
            }).ToList(); ;
        }
    }
}
