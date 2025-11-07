using MASsenger.Application.Commands.BaseUserCommands;
using MASsenger.Application.Dto.Create;
using MASsenger.Application.Dto.Read;
using MASsenger.Application.Dto.Update;
using MASsenger.Application.Queries.BaseUserQueries;
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

        [HttpGet("getAllUsers")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok((await _sender.Send(new GetAllUsersQuery())).Select(u => new UserReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Description = u.Description,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified 
            }).ToList());
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

        [HttpGet("getAllBots")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BotReadDto>))]
        public async Task<IActionResult> GetAllBots()
        {
            return Ok((await _sender.Send(new GetAllBotsQuery())).Select(u => new BotReadDto
            {
                Id = u.Id,
                Name = u.Name,
                Username = u.Username,
                Description = u.Description,
                Token = u.Token,
                CreatedAt = u.CreatedAt,
                IsVerified = u.IsVerified,
                IsActive = u.IsActive
            }).ToList());
        }

        [HttpPost("addBot")]
        public async Task<IActionResult> AddBotAsync([FromBody] BotCreateDto bot, UInt64 ownerId)
        {
            if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.Done) return Ok("Bot added successfully.");
            else if (await _sender.Send(new AddBotCommand(bot, ownerId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid Owner Id.");
            return BadRequest("Something went wrong while saving the bot.");
        }

        [HttpPut("updateBot")]
        public async Task<IActionResult> UpdateBotAsync(BotUpdateDto bot)
        {
            if (await _sender.Send(new UpdateBotCommand(bot)) == Core.Enums.TransactionResultType.Done) return Ok("Bot updated successfully.");
            else if (await _sender.Send(new UpdateBotCommand(bot)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid bot Id.");
            return BadRequest("Something went wrong while updating the bot.");
        }

        [HttpDelete("deleteBot")]
        public async Task<IActionResult> DeleteBotAsync(UInt64 botId)
        {
            if (await _sender.Send(new DeleteBotCommand(botId)) == Core.Enums.TransactionResultType.Done) return Ok("Bot deleted successfully.");
            else if (await _sender.Send(new DeleteBotCommand(botId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid bot Id.");
            return BadRequest("Something went wrong while deleting the bot.");
        }
    }
}
