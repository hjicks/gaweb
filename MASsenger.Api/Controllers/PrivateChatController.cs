using MASsenger.Application.Commands.ChannelChatCommands;
using MASsenger.Application.Commands.PrivateChatCommands;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Queries.PrivateChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Bot")]

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
            return Ok(await _sender.Send(new GetAllPrivateChatsQuery()));
        }

        [HttpPost, AllowAnonymous]
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
