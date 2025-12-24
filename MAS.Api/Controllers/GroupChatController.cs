using MAS.Application.Commands.GroupChatCommands;
using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Queries.GroupChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace MAS.Api.Controllers;

[Route("api/group-chats")]
[ApiController]
[Authorize(Roles = "User")]
public class GroupChatController : BaseController
{
    public GroupChatController(ISender sender) : base(sender)
    {

    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllGroupChatsAsync()
    {
        var result = await _sender.Send(new GetAllGroupChatsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUserGroupChatsAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new GetAllUserGroupChatsQuery(userId));
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{groupChatId}")]
    public async Task<IActionResult> GetGroupChatAsync(int groupChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new GetGroupChatQuery(userId, groupChatId));
        if (result.Ok)
        {
            Log.Information($"Group chat {groupChatId} information fetched.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("members/{groupChatId}")]
    public async Task<IActionResult> GetGroupChatMembersAsync(int groupChatId)
    {
        var result = await _sender.Send(new GetGroupChatMembersQuery(groupChatId));
        if (result.Ok)
        {
            Log.Information($"Members of group chat {groupChatId} fetched.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddGroupChatAsync(GroupChatAddDto groupChat)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddGroupChatCommand(ownerId, groupChat));
        if (result.Ok)
        {
            Log.Information($"User {ownerId} added group {groupChat.Groupname}.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGroupChatAsync(GroupChatUpdateDto groupChat)
    {
        var result = await _sender.Send(new UpdateGroupChatCommand(groupChat));
        if (result.Ok)
        {
            Log.Information($"Group {groupChat.Id} information updated.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{groupChatId}")]
    public async Task<IActionResult> DeleteGroupChatAsync(int groupChatId)
    {
        var result = await _sender.Send(new DeleteGroupChatCommand(groupChatId));
        if (result.Ok)
        {
            Log.Information($"Group chat {groupChatId} deleted.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }
}
