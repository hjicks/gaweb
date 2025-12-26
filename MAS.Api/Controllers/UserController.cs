using MAS.Application.Commands.UserCommands;
using MAS.Application.Dtos.UserDtos;
using MAS.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

    [SwaggerOperation(
    Summary = "Get all users",
    Description = "Returns all users in the system. Accessible only by admins."
    )]
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var result = await _sender.Send(new GetAllUsersQuery());
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Add a user",
    Description = "Creates a new user."
    )]
    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> AddUserAsync([FromBody] UserAddDto user)
    {
        var result = await _sender.Send(new AddUserCommand(user));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Update a user",
    Description = "Updates the details of the authenticated user."
    )]
    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new UpdateUserCommand(userId, user));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Update user last seen",
    Description = "Updates the last seen information of the authenticated user."
    )]
    [HttpPut("last-seen")]
    public async Task<IActionResult> UpdateUserLastSeenAsync(UserLastSeenUpdateDto user)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new UpdateUserLastSeenCommand(userId, user));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Delete a user",
    Description = "Deletes the authenticated user."
    )]
    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new DeleteUserCommand(userId));
        return StatusCode(result.StatusCode, result);
    }
}
