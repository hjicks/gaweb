using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Interfaces;
using MediatR;

namespace MASsenger.Application.Queries.MessageQueries
{
    public record GetAllSystemMessagesQuery() : IRequest<IEnumerable<SystemMessageReadDto>>;
    public class GetAllSystemMessagesQueryHandler : IRequestHandler<GetAllSystemMessagesQuery, IEnumerable<SystemMessageReadDto>>
    {
        private readonly ISystemMessageRepository _systemMessageRepository;
        public GetAllSystemMessagesQueryHandler(ISystemMessageRepository messageRepository)
        {
            _systemMessageRepository = messageRepository;
        }
        public async Task<IEnumerable<SystemMessageReadDto>> Handle(GetAllSystemMessagesQuery request, CancellationToken cancellationToken)
        {
            return (await _systemMessageRepository.GetAllAsync()).Select(m => new SystemMessageReadDto
            {
                Text = m.Text,
                SentTime = m.SentTime,
                DestinationID = m.Destination.Id,
            }).ToList(); ;
        }
    }
}
