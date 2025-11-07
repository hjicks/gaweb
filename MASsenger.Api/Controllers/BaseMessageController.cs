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
        private readonly IBaseUserRepository _baseUserRepository;
        private readonly IBaseChatRepository _baseChatRepository;

        public BaseMessageController(IBaseMessageRepository baseMessageRepository, IBaseUserRepository baseUserRepository, IBaseChatRepository baseChatRepository)
        {
            _baseMessageRepository = baseMessageRepository;
            _baseUserRepository = baseUserRepository;
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
            //    Destination = u.Destination,
            //    SentTime = u.SentTime,
            //    Text = u.Text
            //}).ToList();
            return Ok(baseMessages);
        }

        [HttpPost("addMessage")]
        public async Task<IActionResult> AddBaseMessageAsync([FromBody] BaseMessageDto baseMessage, UInt64 senderId, UInt64 destinationChatId)
        {
            var sender = await _baseUserRepository.GetUserByIdAsync(senderId);
            if (sender == null)
                return BadRequest("Invalid owner id.");
            var destinationChat = await _baseChatRepository.GetBaseChatByIdAsync(destinationChatId);
            if (destinationChat == null)
                return BadRequest("Invalid chat id.");

            var newBaseMessage = new BaseMessage
            {
                Text = baseMessage.Text
            };

            if (await _baseMessageRepository.AddBaseMessageAsync(newBaseMessage, sender, destinationChat)) return Ok("Message added successfully.");
            return BadRequest("Something went wrong while saving the message.");
        }
    }
}
