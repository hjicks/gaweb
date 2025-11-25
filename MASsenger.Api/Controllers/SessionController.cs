using MASsenger.Application.Commands.SessionCommands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISender _sender;
        public SessionController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> RefreshJwt([FromBody] Int32 sessionId)
        {
            Guid.TryParse(Request.Cookies["refreshToken"], out Guid refreshToken);
            var result = await _sender.Send(new RefreshJwtCommand(sessionId, refreshToken));
            if (result.Success)
            {
                Log.Information($"Jwt of user with id {sessionId} renewed.");
                return StatusCode((int)result.StatusCode, result.Response.Jwt);
            }
            Log.Information($"Unsuccessful attempt to refresh session {sessionId}.");
            return StatusCode((int)result.StatusCode, result.Description);
        }
    }
}
