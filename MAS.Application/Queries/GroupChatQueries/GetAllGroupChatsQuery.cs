using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Application.Queries.GroupChatQueries
{
    public record GetAllGroupChatsQuery() : IRequest<IEnumerable<GroupChatGetDto>>;
    public class GetAllChannelChatsQueryHandler : IRequestHandler<GetAllGroupChatsQuery, IEnumerable<GroupChatGetDto>>
    {
        private readonly IGroupChatRepository _channelChatRepository;
        public GetAllChannelChatsQueryHandler(IGroupChatRepository channelChatRepository)
        {
            _channelChatRepository = channelChatRepository;
        }
        public async Task<IEnumerable<GroupChatGetDto>> Handle(GetAllGroupChatsQuery request, CancellationToken cancellationToken)
        {
            return (await _channelChatRepository.GetAllAsync()).Select(c => new GroupChatGetDto
            {
                Id = c.Id,
                DisplayName = c.DisplayName,
                Groupname = c.Groupname,
                Description = c.Description,
                Avatar = c.Avatar,
                CreatedAt = c.CreatedAt,
                MsgPermissionType = c.MsgPermissionType
            }).ToList();
        }
    }
}
