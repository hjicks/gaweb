using MASsenger.Core.Dto;
using MASsenger.Core.Entities;
using MASsenger.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseUserController : ControllerBase
    {
        private readonly IBaseUserRepository _baseUserRepository;
        public BaseUserController(IBaseUserRepository baseUserRepository)
        {
            _baseUserRepository = baseUserRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BaseUser>))]
        public async Task<IActionResult> GetBaseUsers()
        {
            var baseUsers = await _baseUserRepository.GetBaseUsersAsync();

            return Ok(baseUsers);
        }

        [HttpPost("addBot")]
        public async Task<IActionResult> AddBotAsync([FromBody] BotDto bot, UInt64 ownerId)
        {
            var owner = await _baseUserRepository.GetUserByIdAsync(ownerId);
            if (owner == null)
                return BadRequest("Invalid Owner Id.");

            var newBot = new Bot
            {
                Name = bot.Name,
                Username = bot.Username,
                Description = bot.Description,
                Token = bot.Token
            };

            if (await _baseUserRepository.AddBotAsync(newBot, owner)) return Ok("Bot added successfully.");
            return BadRequest("Something went wrong while saving the bot.");
        }

        [HttpPost("addUser")]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto user)
        {
            var newUser = new User
            {
                Name = user.Name,
                Username = user.Username,
                Description = user.Description
            };
            if (await _baseUserRepository.AddUserAsync(newUser)) return Ok("User added successfully.");
            return BadRequest("Something went wrong while saving the user.");
        }
    }
}
