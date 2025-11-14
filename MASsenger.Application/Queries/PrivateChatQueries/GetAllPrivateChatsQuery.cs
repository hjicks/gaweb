using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Application.Queries.PrivateChatQueries
{
    public record GetAllPrivateChatsQuery() : IRequest<IEnumerable<PrivateChatReadDto>>;
    public class GetAllPrivateChatsQueryHandler : IRequestHandler<GetAllPrivateChatsQuery, IEnumerable<PrivateChatReadDto>>
    {
        private readonly IPrivateChatRepository _privateChatRepository;
        public GetAllPrivateChatsQueryHandler(IPrivateChatRepository privateChatRepository)
        {
            _privateChatRepository = privateChatRepository;
        }
        public async Task<IEnumerable<PrivateChatReadDto>> Handle(GetAllPrivateChatsQuery request, CancellationToken cancellationToken)
        {
            return (await _privateChatRepository.GetAllAsync()).Select(c => new PrivateChatReadDto
            {
                Id = c.Id,
                CreatedAt = c.CreatedAt,
            }).ToList();
        }
    }
}
