using MASsenger.Application.Commands.SessionCommands;
using MASsenger.Application.Commands.UserCommands;
using MASsenger.Application.Dtos.Create;
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
        public async Task<IActionResult> RefreshJwt([FromBody] Int32 userId)
        {
            Guid refreshToken = Guid.Parse(Request.Cookies["refreshToken"]);
            var jwt = await _sender.Send(new RefreshJwtCommand(userId, refreshToken));
            Log.Information($"Jwt of user with id {userId} renewed.");
            return Ok(jwt);
        }
    }
}
