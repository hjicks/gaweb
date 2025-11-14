using MASsenger.Application.Commands.MessageCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Queries.MessageQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User,Bot")]
    public class MessageController : ControllerBase
    {
        private readonly ISender _sender;
        public MessageController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageReadDto>))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllMessages()
        {
            return Ok(await _sender.Send(new GetAllMessagesQuery()));
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddMessageAsync([FromBody] MessageCreateDto msg)
        {
            if (await _sender.Send(new AddMessageCommand(msg)) == Core.Enums.TransactionResultType.Done)
            {
                Log.Information("Message added.");
                return Ok("Message added successfully.");
            }
            return BadRequest("Something went wrong while saving the message.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMessageAsync(MessageUpdateDto msg)
        {
            if (await _sender.Send(new UpdateMessageCommand(msg)) == Core.Enums.TransactionResultType.Done)
            {
                Log.Information($"Message {msg.Id} updated.");
                return Ok("Message updated successfully.");
            }
            else if (await _sender.Send(new UpdateMessageCommand(msg)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid message Id.");
            return BadRequest("Something went wrong while updating the message.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessageAsync(Int32 msgId)
        {
            if (await _sender.Send(new DeleteMessageCommand(msgId)) == Core.Enums.TransactionResultType.Done)
            {
                Log.Information($"Message {msgId} deleted.");
                return Ok("Message deleted successfully.");
            }
            else if (await _sender.Send(new DeleteMessageCommand(msgId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while deleting the message.");
        }
    }
}
