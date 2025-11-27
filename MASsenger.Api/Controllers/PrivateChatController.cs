using MASsenger.Application.Commands.ChannelChatCommands;
using MASsenger.Application.Commands.PrivateChatCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Queries.ChannelChatQueries;
using MASsenger.Application.Queries.PrivateChatQueries;
using MASsenger.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateChatController : ControllerBase
    {
        private readonly ISender _sender;
        public PrivateChatController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PrivateChatReadDto>))]
        public async Task<IActionResult> GetAllPrivateChats()
        {
            return Ok(await _sender.Send(new GetAllPrivateChatsQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> AddPrivateChatAsync(Int32 starterId, Int32 receiverId)
        {
            if (await _sender.Send(new AddPrivateChatCommand(starterId, receiverId)) == Core.Enums.TransactionResultType.Done) return Ok("PrivateChat added successfully.");
            else if (await _sender.Send(new AddPrivateChatCommand(starterId, receiverId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id(s)");
            return BadRequest("Something went wrong while saving the privateChat.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePrivateChatAsync(Int32 privateChatId)
        {
            if (await _sender.Send(new DeletePrivateChatCommand(privateChatId)) == Core.Enums.TransactionResultType.Done) return Ok("PrivateChat deleted successfully.");
            else if (await _sender.Send(new DeleteChannelChatCommand(privateChatId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid privateChat Id.");
            return BadRequest("Something went wrong while deleting the privateChat.");
        }
    }
}
