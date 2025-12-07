using MASsenger.Application.Commands.MessageCommands;
using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Queries.MessageQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace MASsenger.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class MessageController : BaseController
    {
        public MessageController(ISender sender) : base(sender)
        {

        }

        [HttpGet("messages")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMessages()
        {
            var result = await _sender.Send(new GetAllMessagesQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("chat/message")]
        public async Task<IActionResult> AddMessageAsync([FromBody] MessageCreateDto message)
        {
            var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new AddMessageCommand(senderId, message));
            if (result.Ok)
            {
                Log.Information($"User {senderId} added message to chat {message.DestinationId}.");
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("chat/message")]
        public async Task<IActionResult> UpdateMessageAsync([FromBody] MessageUpdateDto message)
        {
            var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new UpdateMessageCommand(senderId, message));
            if (result.Ok)
            {
                Log.Information($"User {senderId} updated message {message.Id}.");
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("chat/message/{messageId}")]
        public async Task<IActionResult> DeleteMessageAsync(int messageId)
        {
            var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new DeleteMessageCommand(senderId, messageId));
            if (result.Ok)
            {
                Log.Information($"User {senderId} deleted message {messageId}.");
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
