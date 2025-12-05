using MASsenger.Application.Commands.BotCommands;
using MASsenger.Application.Dtos.BotDtos;
using MASsenger.Application.Queries.BotQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    /*
     * only users, specifically, owners of the bots may deal with those endpoints,
     * with exception the of Login()
     */
    [Authorize(Roles = "User")]
    public class BotController : BaseController
    {
        public BotController(ISender sender) : base(sender) 
        {

        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(BotLoginDto botCerd)
        {
            if (await _sender.Send(new LoginBotQuery(botCerd)) == "error") return BadRequest("Something went wrong while logging in.");
            Log.Information($"Bot logged in.");
            return Ok(await _sender.Send(new LoginBotQuery(botCerd)));
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
