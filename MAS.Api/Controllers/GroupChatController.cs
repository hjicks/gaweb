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

    [HttpGet("{groupChatId}")]
    public async Task<IActionResult> GetGroupChatAsync(int groupChatId) // may need to check who is fetching this
    {
        var result = await _sender.Send(new GetGroupChatQuery(groupChatId));
        if (result.Ok)
        {
            //Log.Information($"User {} got group chat {groupChatId} data.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("members/{groupChatId}")]
    public async Task<IActionResult> GetGroupChatMembersAsync(int groupChatId) // may need to check who is fetching this
    {
        var result = await _sender.Send(new GetGroupChatMembersQuery(groupChatId));
        if (result.Ok)
        {
            //Log.Information($"User {} got members of group chat {groupChatId}.");
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddGroupChatAsync(PublicGroupChatAddDto groupChat)
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
    public async Task<IActionResult> UpdateGroupChatAsync(PublicGroupChatUpdateDto groupChat)
    {
        if (await _sender.Send(new UpdateGroupChatCommand(groupChat)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat updated successfully.");
        else if (await _sender.Send(new UpdateGroupChatCommand(groupChat)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid channelChat Id.");
        return BadRequest("Something went wrong while updating the channelChat.");
    }

    [HttpDelete("{groupChatId}")]
    public async Task<IActionResult> DeleteGroupChatAsync(int groupChatId)
    {
        if (await _sender.Send(new DeleteGroupChatCommand(groupChatId)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat deleted successfully.");
        else if (await _sender.Send(new DeleteGroupChatCommand(groupChatId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid channelChat Id.");
        return BadRequest("Something went wrong while deleting the channelChat.");
    }
}
