using MAS.Application.Commands.GroupChatCommands;
using MAS.Application.Commands.PrivateChatCommands;
using MAS.Application.Queries.PrivateChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllPrivateChatsAsync()
    {
        var result =  await _sender.Send(new GetAllPrivateChatsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserChatsAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new GetAllUserPrivateChatsQuery(userId));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{receiverId}")]
    public async Task<IActionResult> AddPrivateChatAsync(int receiverId)
    {
        var starterId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddPrivateChatCommand(starterId, receiverId));
        if (result.Ok)
        {
            Log.Information($"Private chat with starter id {starterId} and receiver id {receiverId} added.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{privateChatId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePrivateChatAsync(int privateChatId)
    {
        var result = await _sender.Send(new DeletePrivateChatCommand(privateChatId));
        if (result.Ok)
        {
            Log.Information($"Private chat {privateChatId} deleted.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{privateChatId}/members")]
    public async Task<IActionResult> LeavePrivateChatAsync(int privateChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new LeavePrivateChatCommand(userId, privateChatId));
        if (result.Ok)
        {
            Log.Information($"User {userId} left private chat {privateChatId}.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }
}
