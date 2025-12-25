using MAS.Application.Commands.UserCommands;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace MAS.Api.Controllers;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "User")]
public class UserController : BaseController
{
    public UserController(ISender sender) : base(sender)
    {

    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var result = await _sender.Send(new GetAllUsersQuery());
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> AddUserAsync([FromBody] UserAddDto user)
    {
        var result = await _sender.Send(new AddUserCommand(user));
        if (result.Ok)
        {
            Log.Information($"User {user.Username} added.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
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

    [HttpPut("last-seen")]
    public async Task<IActionResult> UpdateUserLastSeenAsync(UserLastSeenUpdateDto user)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new UpdateUserLastSeenCommand(userId, user));
        if (result.Ok)
        {
            Log.Information($"User {userId} last seen updated.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
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
