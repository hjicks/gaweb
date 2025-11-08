using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using MASsenger.Core.Entities.Message;
using MediatR;

namespace MASsenger.Application.Queries.BaseMessageQueries
{
    public record GetAllSystemMessagesQuery() : IRequest<IEnumerable<SystemMessage>>;
    public class GetSystemMessagesCommandHandler : IRequestHandler<GetAllSystemMessagesQuery, IEnumerable<SystemMessage>>
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        public GetSystemMessagesCommandHandler(IBaseMessageRepository baseMessageRepository)
        {
            _baseMessageRepository = baseMessageRepository;
        }
        public async Task<IEnumerable<SystemMessage>> Handle(GetAllSystemMessagesQuery request, CancellationToken cancellationToken)
        {
            return await _baseMessageRepository.GetAllSystemMessagesAsync();
        }
    }
}
