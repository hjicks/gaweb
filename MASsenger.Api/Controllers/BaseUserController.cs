using MASsenger.Application.Commands;
using MASsenger.Application.Queries;
using MASsenger.Core.Dto;
using MASsenger.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseUserController : ControllerBase
    {
        private readonly ISender _sender;
        public BaseUserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BaseUser>))]
        public async Task<IActionResult> GetBaseUsers()
        {
            return Ok(await _sender.Send(new GetBaseUsersQuery()));
        }

        [HttpPost("addBot")]
        public async Task<IActionResult> AddBotAsync([FromBody] BotDto bot, UInt64 ownerId)
        {
            if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.Done) return Ok("Bot added successfully.");
            else if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Owner Id.");
            return BadRequest("Something went wrong while saving the bot.");
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto user)
        {
            if (await _sender.Send(new AddUserCommand(user)) == Core.Enums.TransactionResultType.Done) return Ok("User added successfully.");
            return BadRequest("Something went wrong while saving the user.");
        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
        {
            if (await _sender.Send(new UpdateUserCommand(user)) == Core.Enums.TransactionResultType.Done) return Ok("User updated successfully.");
            else if (await _sender.Send(new UpdateUserCommand(user)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while updating the user.");
        }

        [HttpDelete("deleteUser")]
        public async Task<IActionResult> DeleteUserAsync(UInt64 userId)
        {
            if (await _sender.Send(new DeleteUserCommand(userId)) == Core.Enums.TransactionResultType.Done) return Ok("User deleted successfully.");
            else if (await _sender.Send(new DeleteUserCommand(userId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while deleting the user.");
        }
    }
}
