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
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("refreshToken", result.Response.RefreshToken, cookieOptions);
                Log.Information($"User {userCred.Username} logged in.");
                return StatusCode((int)result.StatusCode, result.Response.Jwt);
            }
            Log.Information($"Unsuccessful login attempt with username {userCred.Username}.");
            return StatusCode((int)result.StatusCode, result.Description);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _sender.Send(new GetAllUsersQuery());
            return StatusCode((int)result.StatusCode, result.Response.Entities);
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto user)
        {
            var result = await _sender.Send(new AddUserCommand(user));
            if (result.Success)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("refreshToken", result.Response.RefreshToken, cookieOptions);
                Log.Information($"User {user.Username} added.");
                return StatusCode((int)result.StatusCode, result.Response.Jwt);
            }
            return StatusCode((int)result.StatusCode, result.Description);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new UpdateUserCommand(userId, user));
            if (result.Success)
            {
                Log.Information($"User {userId} updated.");
                return StatusCode((int)result.StatusCode, result.Description);
            }
            return StatusCode((int)result.StatusCode, result.Description);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new DeleteUserCommand(userId));
            if (result.Success)
            {
                Log.Information($"User {userId} deleted.");
                return StatusCode((int)result.StatusCode, result.Description);
            }
            return StatusCode((int)result.StatusCode, result.Description);
        }
    }
}
