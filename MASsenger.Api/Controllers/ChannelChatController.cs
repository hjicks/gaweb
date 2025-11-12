using MASsenger.Application.Commands.ChannelChatCommands;
using MASsenger.Application.Dtos.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelChatController : ControllerBase
    {
        private readonly ISender _sender;
        public ChannelChatController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> AddChannelChatAsync([FromBody] ChannelChatCreateDto channelChat, Int32 ownerId)
        {
            if (await _sender.Send(new AddChannelChatCommand(channelChat, ownerId)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat added successfully.");
            else if (await _sender.Send(new AddChannelChatCommand(channelChat, ownerId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Owner Id.");
            return BadRequest("Something went wrong while saving the channelChat.");
        }
    }
}
