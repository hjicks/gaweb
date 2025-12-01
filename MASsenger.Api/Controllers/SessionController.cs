using MASsenger.Application.Commands.SessionCommands;
using MASsenger.Application.Dtos.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class SessionController : BaseController
    {
        public SessionController(ISender sender) : base(sender)
        {

        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userCred)
        {
            var result = await _sender.Send(new LoginCommand(userCred));
            if (result.Success)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Append("refreshToken", result.Response.RefreshToken, cookieOptions);
                Log.Information($"User {userCred.Username} logged in.");
                return StatusCode(result.StatusCode, new { result.Success, result.Response });
            }
            Log.Information($"Unsuccessful login attempt with username {userCred.Username}.");
            return StatusCode(result.StatusCode, new { result.Success, result.Description });
        }

        [HttpPut("logout")]
        public async Task<IActionResult> Logout([FromBody] Int32 sessionId)
        {
            var result = await _sender.Send(new LogoutCommand(sessionId));
            if (result.Success)
            {
                Log.Information($"Session with id {sessionId} is expired.");
                return StatusCode(result.StatusCode, new { result.Success, result.Response });
            }
            return StatusCode(result.StatusCode, new { result.Success, result.Description });
        }

        [HttpPost("refresh"), AllowAnonymous]
        public async Task<IActionResult> RefreshJwt([FromBody] Int32 sessionId)
        {
            Guid.TryParse(Request.Cookies["refreshToken"], out Guid refreshToken);
            var result = await _sender.Send(new RefreshJwtCommand(sessionId, refreshToken));
            if (result.Success)
            {
                Log.Information($"Jwt of user with id {sessionId} renewed.");
                return StatusCode(result.StatusCode, new { result.Success, result.Response });
            }
            Log.Information($"Unsuccessful attempt to refresh session {sessionId}.");
            return StatusCode(result.StatusCode, new { result.Success, result.Description });
        }
    }
}
