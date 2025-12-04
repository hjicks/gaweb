using MASsenger.Application.Commands.ChannelChatCommands;
using MASsenger.Application.Commands.PrivateChatCommands;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Queries.PrivateChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]

    public class PrivateChatController : BaseController
    {
        public PrivateChatController(ISender sender) : base(sender)
        {

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PrivateChatReadDto>))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllPrivateChats()
        {
            var result =  await _sender.Send(new GetAllPrivateChatsQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("getAllUser")]
        public async Task<IActionResult> GetAllUserChatsAsync(Int32 userId)
        {
            var result = await _sender.Send(new GetAllUserPrivateChatsQuery(userId));
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<IActionResult> AddPrivateChatAsync(Int32 receiverId)
        {
            var starterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new AddPrivateChatCommand(starterId, receiverId));
            if (result.Ok)
            {
                Log.Information($"Private chat with starter id {starterId} and receiver id {receiverId} added.");
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
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
