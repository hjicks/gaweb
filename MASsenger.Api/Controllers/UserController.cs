using MASsenger.Application.Commands.UserCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userCred)
        {
            var result = await _sender.Send(new LoginUserCommand(userCred));
            if (result.Success)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.Now.AddDays(7)
                };
                Response.Cookies.Append("refreshToken", result.Response.RefreshToken, cookieOptions);
                Log.Information($"User {userCred.Username} logged in.");
                return StatusCode((int)result.StatusCode, result.Response.Jwt);
            }
            return StatusCode((int)result.StatusCode, result.Response.Message);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _sender.Send(new GetAllUsersQuery()));
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto user)
        {
            (var jwt, var refreshToken) = await _sender.Send(new AddUserCommand(user));
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            Log.Information($"User {user.Username} added.");
            return Ok(jwt);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
        {
            if (await _sender.Send(new UpdateUserCommand(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), user)) == Core.Enums.TransactionResultType.Done)
            {
                Log.Information($"User {User.FindFirstValue(ClaimTypes.NameIdentifier)} updated.");
                return Ok("User updated successfully.");
            }
            else if (await _sender.Send(new UpdateUserCommand(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), user)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while updating the user.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync()
        {
            if (await _sender.Send(new DeleteUserCommand(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))) == Core.Enums.TransactionResultType.Done)
            {
                Log.Information($"User {User.FindFirstValue(ClaimTypes.NameIdentifier)} deleted.");
                return Ok("User deleted successfully.");
            }
            else if (await _sender.Send(new DeleteUserCommand(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)))) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while deleting the user.");
        }
    }
}
