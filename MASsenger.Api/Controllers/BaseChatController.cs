using MASsenger.Application.Dto;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseChatController : ControllerBase
    {
        private readonly IBaseChatRepository _baseChatRepository;
        private readonly IBaseUserRepository _baseUserRepository;

        public BaseChatController(IBaseChatRepository baseChatRepository, IBaseUserRepository baseUserRepository)
        {
            _baseChatRepository = baseChatRepository;
            _baseUserRepository = baseUserRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChannelGroupChat>))]
        public async Task<IActionResult> GetChannelGroupChats()
        {
            var ChannelGroupChats = await _baseChatRepository.GetChannelGroupChatsAsync();

            return Ok(ChannelGroupChats);
        }

        [HttpPost("addChannelGroup")]
        public async Task<IActionResult> AddBotAsync([FromBody] ChannelGroupChatDto channelGroupChat, UInt64 ownerId, UInt64? linkedChannelGroupId)
        {
            var owner = await _baseUserRepository.GetUserByIdAsync(ownerId);
            if (owner == null)
                return BadRequest("Invalid Owner Id.");

            var newChannelGroupChat = new ChannelGroupChat
            {
                Name = channelGroupChat.Name,
                Username = channelGroupChat.Username,
                Description = channelGroupChat.Description
            };

            if (await _baseChatRepository.AddChannelGroupChatAsync(newChannelGroupChat, owner)) return Ok("Channel/Group chat added successfully.");
            return BadRequest("Something went wrong while saving the Channel/Group chat.");
        }
    }
}
