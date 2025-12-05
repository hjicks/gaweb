using MASsenger.Application.Dtos.ChannelChatDtos;
using MASsenger.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Application.Queries.ChannelChatQueries
{
    public record GetAllChannelChatsQuery() : IRequest<IEnumerable<ChannelChatReadDto>>;
    public class GetAllChannelChatsQueryHandler : IRequestHandler<GetAllChannelChatsQuery, IEnumerable<ChannelChatReadDto>>
    {
        private readonly IChannelChatRepository _channelChatRepository;
        public GetAllChannelChatsQueryHandler(IChannelChatRepository channelChatRepository)
        {
            _channelChatRepository = channelChatRepository;
        }
        public async Task<IEnumerable<ChannelChatReadDto>> Handle(GetAllChannelChatsQuery request, CancellationToken cancellationToken)
        {
            return (await _channelChatRepository.GetAllAsync()).Select(c => new ChannelChatReadDto
            {
                Id = c.Id,
                Name = c.Name,
                Username = c.Username,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
            }).ToList();
        }
    }
}
