using MAS.Application.Commands.GroupChatCommands;
using MAS.Application.Commands.PrivateChatCommands;
using MAS.Application.Queries.PrivateChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace MAS.Api.Controllers;

[Route("api/private-chats")]
[ApiController]
[Authorize(Roles = "User")]

public class PrivateChatController : BaseController
{
    public PrivateChatController(ISender sender) : base(sender)
    {

    }

    [SwaggerOperation(
    Summary = "Get all private chats",
    Description = "Returns all private chats in the system. Accessible only by admins."
    )]
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllPrivateChatsAsync()
    {
        var result =  await _sender.Send(new GetAllPrivateChatsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Get user's private chats",
    Description = "Returns all private chats of the authenticated user."
    )]
    [HttpGet]
    public async Task<IActionResult> GetAllUserChatsAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new GetAllUserPrivateChatsQuery(userId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Create a private chat",
    Description = "Creates a new private chat between the authenticated user and the specified user."
    )]
    [HttpPost("{receiverId}")]
    public async Task<IActionResult> AddPrivateChatAsync(int receiverId)
    {
        var starterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddPrivateChatCommand(starterId, receiverId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Delete a private chat",
    Description = "Deletes the specified private chat. Accessible only by admins."
    )]
    [HttpDelete("{privateChatId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePrivateChatAsync(int privateChatId)
    {
        var result = await _sender.Send(new DeletePrivateChatCommand(privateChatId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Leave a private chat",
    Description = "Removes the authenticated user from the specified private chat."
    )]
    [HttpDelete("{privateChatId}/members")]
    public async Task<IActionResult> LeavePrivateChatAsync(int privateChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new LeavePrivateChatCommand(userId, privateChatId));
        return StatusCode(result.StatusCode, result);
    }
}
