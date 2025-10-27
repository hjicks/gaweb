using MASsenger.Application.Commands.BaseUserCommands;
using MASsenger.Application.Queries.BaseUserQueries;
using MASsenger.Core.Dto.Create;
using MASsenger.Core.Dto.Read;
using MASsenger.Core.Dto.Update;
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        public async Task<IActionResult> GetUsers()
        {
            return Ok((await _sender.Send(new GetUsersQuery())).Select(u => new UserReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Description = u.Description,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified 
            }).ToList());
        }

        [HttpPost("addBot")]
        public async Task<IActionResult> AddBotAsync([FromBody] BotCreateDto bot, UInt64 ownerId)
        {
            if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.Done) return Ok("Bot added successfully.");
            else if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Owner Id.");
            return BadRequest("Something went wrong while saving the bot.");
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto user)
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
