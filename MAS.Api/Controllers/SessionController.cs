using MAS.Application.Commands.SessionCommands;
using MAS.Application.Dtos.SessionDtos;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Queries.SessionQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MAS.Api.Controllers;

[Route("api/sessions")]
[ApiController]
[Authorize(Roles = "User")]
public class SessionController : BaseController
{
    public SessionController(ISender sender) : base(sender)
    {

    }

    [SwaggerOperation(
    Summary = "Get all sessions",
    Description = "Returns all active sessions in the system. Accessible only by admins."
    )]
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllSessionsAsync()
    {
        var result = await _sender.Send(new GetAllSessionsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Login",
    Description = "Authenticates a user and creates a new session."
    )]
    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> LoginAsync(UserLoginDto userCred)
    {
        var result = await _sender.Send(new LoginCommand(userCred));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Logout",
    Description = "Ends the specified session."
    )]
    [HttpPut("logout/{sessionId}")]
    public async Task<IActionResult> LogoutAsync(int sessionId)
    {
        var result = await _sender.Send(new LogoutCommand(sessionId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Refresh JWT",
    Description = "Generates a new JWT using a refresh token."
    )]
    [HttpPost("refresh"), AllowAnonymous]
    public async Task<IActionResult> RefreshJwtAsync(SessionRefreshTokenDto tokenDto)
    {
        var result = await _sender.Send(new RefreshSessionCommand(tokenDto));
        return StatusCode(result.StatusCode, result);
    }
}
