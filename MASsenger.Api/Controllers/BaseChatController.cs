using MASsenger.Core.Dto;
using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using MASsenger.Infrastracture.Repositories;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ChannelGroupChat))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetChannelGroupChatById(UInt64 id)
        {
            var chat = await _baseChatRepository.GetChannelGroupChatByIdAsync(id);
            if (chat == null)
                return NotFound("Channel/Group chat not found.");

            return Ok(chat);
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

        [HttpPut("updateChannelGroup")]
        public async Task<IActionResult> UpdateChannelGroupChat(UInt64 id, [FromBody] ChannelGroupChatDto updatedDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existing = await _baseChatRepository.GetChannelGroupChatByIdAsync(id);
            if (existing == null) return NotFound("Channel/Group chat not found.");

            existing.Name = updatedDto.Name;
            existing.Username = updatedDto.Username;
            existing.Description = updatedDto.Description;

            var ok = await _baseChatRepository.UpdateChannelGroupChatAsync(existing);
            if (ok) return Ok("Channel/Group chat updated successfully.");
            return BadRequest("Failed to update the Channel/Group chat.");
        }

        [HttpDelete("deleteChannelGroup")]
        public async Task<IActionResult> DeleteChannelGroupChat(UInt64 id)
        {
            var existing = await _baseChatRepository.GetChannelGroupChatByIdAsync(id);
            if (existing == null) return NotFound("Channel/Group chat not found.");

            var ok = await _baseChatRepository.DeleteChannelGroupChatAsync(existing);
            if (ok) return Ok("Channel/Group chat deleted successfully.");
            return BadRequest("Failed to delete the Channel/Group chat.");
        }
    }
}
