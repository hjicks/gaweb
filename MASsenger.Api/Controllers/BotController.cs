using MASsenger.Application.Commands.BotCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Queries.BotQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly ISender _sender;
        public BotController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BotReadDto>))]
        public async Task<IActionResult> GetAllBots()
        {
            return Ok(await _sender.Send(new GetAllBotsQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> AddBotAsync([FromBody] BotCreateDto bot, Int32 ownerId)
        {
            if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.Done) return Ok("Bot added successfully.");
            else if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Owner Id.");
            return BadRequest("Something went wrong while saving the bot.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBotAsync(BotUpdateDto bot)
        {
            if (await _sender.Send(new UpdateBotCommand(bot)) == Core.Enums.TransactionResultType.Done) return Ok("Bot updated successfully.");
            else if (await _sender.Send(new UpdateBotCommand(bot)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid bot Id.");
            return BadRequest("Something went wrong while updating the bot.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBotAsync(Int32 botId)
        {
            if (await _sender.Send(new DeleteBotCommand(botId)) == Core.Enums.TransactionResultType.Done) return Ok("Bot deleted successfully.");
            else if (await _sender.Send(new DeleteBotCommand(botId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid bot Id.");
            return BadRequest("Something went wrong while deleting the bot.");
        }
    }
}
