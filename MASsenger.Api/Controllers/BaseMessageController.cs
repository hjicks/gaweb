using MASsenger.Application.Dto;
using MASsenger.Application.Interfaces;
using MASsenger.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseMessageController : ControllerBase
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        private readonly IBaseChatRepository _baseChatRepository;

        public BaseMessageController(IBaseMessageRepository baseMessageRepository, IBaseChatRepository baseChatRepository)
        {
            _baseMessageRepository = baseMessageRepository;
            _baseChatRepository = baseChatRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BaseMessage>))]
        public async Task<IActionResult> GetBaseMessagesAsync()
        {
            var baseMessages = await _baseMessageRepository.GetBaseMessagesAsync();
            //var baseMessagesDtos = baseMessages.Select(u => new BaseMessageDto
            //{
            //    Id = u.Id,
            //    Sender = u.Sender,
            //    DestinationID = u.Destination,
            //    SentTime = u.SentTime,
            //    Text = u.Text
            //}).ToList();
            return Ok(baseMessages);
        }

        [HttpPost("addMessage")]
        public async Task<IActionResult> AddBaseMessageAsync([FromBody] BaseMessageDto baseMessage, UInt64 destinationId)
        {
            var destinationChat = await _baseChatRepository.GetBaseChatByIdAsync(destinationId);
            if (destinationChat == null)
                return BadRequest("Invalid chat id.");

            var newBaseMessage = new BaseMessage
            {
                Text = baseMessage.Text,
                Destination = destinationChat
            };

            if (await _baseMessageRepository.AddBaseMessageAsync(newBaseMessage)) return Ok("Message added successfully.");
            return BadRequest("Something went wrong while saving the message.");
        }

        [HttpPut("updateMessage")]
        public async Task<IActionResult> AddUpdateAsync(UInt64 id, [FromBody] BaseMessageDto msg)
        {
            var dbMsg = await _baseMessageRepository.GetBaseMessageByIdAsync(id);
            if (dbMsg == null)
                return BadRequest("Invalid message Id.");

            dbMsg.Text = msg.Text;

            if (await _baseMessageRepository.UpdateBaseMessageAsync(dbMsg)) return Ok("Message updated successfully.");
            return BadRequest("Something went wrong while saving the Message.");
        }

        [HttpDelete("deleteMessage")]
        public async Task<IActionResult> AddDeleteAsync(UInt64 msgId)
        {
            var dbMsg = await _baseMessageRepository.GetBaseMessageByIdAsync(msgId);
            if (dbMsg == null)
                return BadRequest("Invalid user Id.");

            if (await _baseMessageRepository.DeleteBaseMessageAsync(dbMsg)) return Ok("Message deleted successfully.");
            return BadRequest("Something went wrong while deleting the message.");
        }
    }
}
