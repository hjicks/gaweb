using MASsenger.Application.Commands.UserCommands;
using MASsenger.Application.Dtos.UserDtos;
using MASsenger.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace MASsenger.Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : BaseController
    {
        public UserController(ISender sender) : base(sender)
        {

        }

        [HttpGet("users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _sender.Send(new GetAllUsersQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("user"), AllowAnonymous]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto user)
        {
            var result = await _sender.Send(new AddUserCommand(user));
            if (result.Ok)
            {
                Log.Information($"User {user.Username} added.");
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new UpdateUserCommand(userId, user));
            if (result.Ok)
            {
                Log.Information($"User {userId} updated.");
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("user")]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new DeleteUserCommand(userId));
            if (result.Ok)
            {
                Log.Information($"User {userId} deleted.");
                return StatusCode(result.StatusCode, result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
