using MASsenger.Application.Commands.BaseMessageCommands;
using MASsenger.Application.Dto.Create;
using MASsenger.Application.Dto.Read;
using MASsenger.Application.Dto.Update;
using MASsenger.Application.Queries.BaseMessageQueries;
using MASsenger.Application.Queries.BaseMessagesQueries;
using MASsenger.Core.Entities.Message;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseMessageController : ControllerBase
    {
        private readonly ISender _sender;
        public BaseMessageController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("getAllMessages")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageReadDto>))]
        public async Task<IActionResult> GetAllMessages()
        {
            return Ok((await _sender.Send(new GetAllMessagesQuery())).Select(m => new MessageReadDto
            {
                Id = m.Id,
                SenderID = m.Sender.Id,
                DestinationID = m.Destination.Id,
                Text = m.Text,
                SentTime = m.SentTime
            }).ToList());
        }

        [HttpPost("addMessage")]
        public async Task<IActionResult> AddMessageAsync([FromBody] MessageCreateDto Message)
        {
            if (await _sender.Send(new AddMessageCommand(Message)) == Core.Enums.TransactionResultType.Done) return Ok("Message added successfully.");
            return BadRequest("Something went wrong while saving the Message.");
        }

        [HttpPut("updateMessage")]
        public async Task<IActionResult> UpdateMessageAsync(MessageUpdateDto Message)
        {
            if (await _sender.Send(new UpdateMessageCommand(Message)) == Core.Enums.TransactionResultType.Done) return Ok("Message updated successfully.");
            else if (await _sender.Send(new UpdateMessageCommand(Message)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Message Id.");
            return BadRequest("Something went wrong while updating the Message.");
        }

        [HttpDelete("deleteMessage")]
        public async Task<IActionResult> DeleteMessageAsync(UInt64 MessageId)
        {
            if (await _sender.Send(new DeleteMessageCommand(MessageId)) == Core.Enums.TransactionResultType.Done) return Ok("Message deleted successfully.");
            else if (await _sender.Send(new DeleteMessageCommand(MessageId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Message Id.");
            return BadRequest("Something went wrong while deleting the Message.");
        }

        [HttpGet("getAllSystemMessages")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SystemMessageReadDto>))]
        public async Task<IActionResult> GetAllSystemMessages()
        {
            return Ok((await _sender.Send(new GetAllSystemMessagesQuery())).Select(m => new SystemMessageReadDto
            {
                Id = m.Id,
                DestinationID = m.Destination.Id,
                Text = m.Text,
                SentTime = m.SentTime
            }).ToList());
        }

        [HttpPost("addSystemMessage")]
        public async Task<IActionResult> AddSystemMessageAsync([FromBody] SystemMessageCreateDto sm)
        {
            if (await _sender.Send(new AddSystemMessageCommand(sm)) == Core.Enums.TransactionResultType.Done) return Ok("SystemMessage added successfully.");
            else if (await _sender.Send(new AddSystemMessageCommand(sm)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Destination Id.");
            return BadRequest("Something went wrong while saving the system message.");
        }
    }
}
