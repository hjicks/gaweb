using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Entities.Message;
using MediatR;

namespace MASsenger.Application.Queries.BaseMessagesQueries
{
    public record GetAllMessagesQuery() : IRequest<IEnumerable<Message>>;
    public class GetMessageCommandHandler : IRequestHandler<GetAllMessagesQuery, IEnumerable<Message>>
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        public GetMessageCommandHandler(IBaseMessageRepository baseMessageRepository)
        {
            _baseMessageRepository = baseMessageRepository;
        }
        public async Task<IEnumerable<Message>> Handle(GetAllMessagesQuery request, CancellationToken cancellationToken)
        {
            return await _baseMessageRepository.GetAllMessagesAsync();
        }
    }
}
