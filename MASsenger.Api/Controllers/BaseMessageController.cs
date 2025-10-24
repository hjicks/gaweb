using MASsenger.Core.Dto;
using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseMessageController : ControllerBase
    {
        private readonly IBaseMessageRepository _baseMessageRepository;
        public BaseMessageController(IBaseMessageRepository baseMessageRepository)
        {
            _baseMessageRepository = baseMessageRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BaseMessage>))]
        public async Task<IActionResult> GetBaseMessages()
        {
            var baseMessages = await _baseMessageRepository.GetBaseMessagesAsync();
            var baseMessagesDtos = baseMessages.Select(u => new BaseMessageDto
            {
                Id = u.Id,
                Sender = u.Sender,
                Destination = u.Destination,
                SentTime = u.SentTime,
                Text = u.Text
            }).ToList();
            return Ok(baseMessagesDtos);
        }
    }
}
