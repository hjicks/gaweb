using MASsenger.Application.Commands.SystemMessageCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Queries.SystemMessageQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SystemMessageController : BaseController
    {
        public SystemMessageController(ISender sender) : base(sender)
        {

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SystemMessageReadDto>))]
        public async Task<IActionResult> GetAllMessages()
        {
            return Ok(await _sender.Send(new GetAllSystemMessagesQuery()));
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddSystemMessageAsync([FromBody] SystemMessageCreateDto msg)
        {
            if (await _sender.Send(new AddSystemMessageCommand(msg)) == Core.Enums.TransactionResultType.Done)
            {
                Log.Information("SystemMessage added.");
                return Ok("SystemMessage added successfully.");
            }
            return BadRequest("Something went wrong while saving the SystemMessage.");
        }
    }
}
