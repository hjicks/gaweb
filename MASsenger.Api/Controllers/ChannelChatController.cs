using MASsenger.Application.Commands.ChannelChatCommands;
using MASsenger.Application.Dtos.ChannelChatDtos;
using MASsenger.Application.Queries.ChannelChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Bot")]
    public class ChannelChatController : BaseController
    {
        public ChannelChatController(ISender sender) : base(sender)
        {

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChannelChatReadDto>))]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllChannelChats()
        {
            return Ok(await _sender.Send(new GetAllChannelChatsQuery()));
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddChannelChatAsync([FromBody] ChannelChatCreateDto channelChat, Int32 ownerId)
        {
            if (await _sender.Send(new AddChannelChatCommand(channelChat, ownerId)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat added successfully.");
            else if (await _sender.Send(new AddChannelChatCommand(channelChat, ownerId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Owner Id.");
            return BadRequest("Something went wrong while saving the channelChat.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChannelChatAsync(ChannelChatUpdateDto channelChat)
        {
            if (await _sender.Send(new UpdateChannelChatCommand(channelChat)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat updated successfully.");
            else if (await _sender.Send(new UpdateChannelChatCommand(channelChat)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid channelChat Id.");
            return BadRequest("Something went wrong while updating the channelChat.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteChannelChatAsync(Int32 channelChatId)
        {
            if (await _sender.Send(new DeleteChannelChatCommand(channelChatId)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat deleted successfully.");
            else if (await _sender.Send(new DeleteChannelChatCommand(channelChatId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid channelChat Id.");
            return BadRequest("Something went wrong while deleting the channelChat.");
        }
    }
}
