using MAS.Application.Commands.SessionCommands;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Queries.SessionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MAS.Api.Controllers;

[Route("api/sessions")]
[ApiController]
[Authorize(Roles = "User")]
public class SessionController : BaseController
{
    public SessionController(ISender sender) : base(sender)
    {

    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllSessionsAsync()
    {
        var result = await _sender.Send(new GetAllSessionsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> LoginAsync(UserLoginDto userCred)
    {
        var result = await _sender.Send(new LoginCommand(userCred));
        if (result.Ok)
        {
            Log.Information($"User {userCred.Username} logged in.");
            return StatusCode(result.StatusCode, result);
        }
        Log.Information($"Unsuccessful login attempt with username {userCred.Username}.");
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("logout/{sessionId}")]
    public async Task<IActionResult> LogoutAsync(int sessionId)
    {
        var result = await _sender.Send(new LogoutCommand(sessionId));
        if (result.Ok)
        {
            Log.Information($"Session with id {sessionId} is expired.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("refresh/{refreshToken}"), AllowAnonymous]
    public async Task<IActionResult> RefreshJwtAsync(string refreshToken)
    {
        var result = await _sender.Send(new RefreshSessionCommand(refreshToken));
        if (result.Ok)
        {
            Log.Information($"Jwt of session with token {refreshToken} renewed.");
            return StatusCode(result.StatusCode, result);
        }
        Log.Information($"Unsuccessful attempt to refresh session with token {refreshToken}.");
        return StatusCode(result.StatusCode, result);
    }
}
