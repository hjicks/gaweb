using MASsenger.Application.Commands.UserCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Queries.UserQueries;
using MASsenger.Application.Responses;
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
    public class UserController : BaseController
    {
        public UserController(ISender sender) : base(sender)
        {

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _sender.Send(new GetAllUsersQuery());
            return StatusCode(result.StatusCode,
                new { result.Ok, Response = result.Response<GetEntityResponse<UserReadDto>>() });
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto user)
        {
            var result = await _sender.Send(new AddUserCommand(user));
            if (result.Ok)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("refreshToken", result.Response<TokensResponse>().RefreshToken, cookieOptions);
                Log.Information($"User {user.Username} added.");
                return StatusCode(result.StatusCode,
                    new { result.Ok, Response = result.Response<TokensResponse>() });
            }
            return StatusCode(result.StatusCode, new { result.Ok, result.Error });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new UpdateUserCommand(userId, user));
            if (result.Ok)
            {
                Log.Information($"User {userId} updated.");
                return StatusCode(result.StatusCode, new { result.Ok, Response = result.Response<BaseResponse>() });
            }
            return StatusCode(result.StatusCode, new { result.Ok, result.Error });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _sender.Send(new DeleteUserCommand(userId));
            if (result.Ok)
            {
                Log.Information($"User {userId} deleted.");
                return StatusCode(result.StatusCode, new { result.Ok, Response = result.Response<BaseResponse>() });
            }
            return StatusCode(result.StatusCode, new { result.Ok, result.Error });
        }
    }
}
