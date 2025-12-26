using MAS.Application.Commands.GroupChatCommands;
using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Queries.GroupChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{groupChatId}/members")]
    public async Task<IActionResult> GetGroupChatMembersAsync(int groupChatId)
    {
        var result = await _sender.Send(new GetGroupChatMembersQuery(groupChatId));
        if (result.Ok)
        {
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
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateGroupChatAsync(GroupChatUpdateDto groupChat)
    {
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new UpdateGroupChatCommand(adminId, groupChat));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{groupChatId}")]
    public async Task<IActionResult> DeleteGroupChatAsync(int groupChatId)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new DeleteGroupChatCommand(ownerId, groupChatId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{groupChatId}/members/join")]
    public async Task<IActionResult> JoinGroupChatAsync(int groupChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new JoinGroupChatCommand(userId, groupChatId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{groupChatId}/members/leave")]
    public async Task<IActionResult> LeaveGroupChatAsync(int groupChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new LeaveGroupChatCommand(userId, groupChatId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("{groupChatId}/members/{memberId}/add")]
    public async Task<IActionResult> AddGroupMemberAsync(int groupChatId, int memberId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddGroupMemberCommand(userId, groupChatId, memberId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{groupChatId}/members/{memberId}/promote")]
    public async Task<IActionResult> PromoteOrDemoteGroupMemberAsync(int groupChatId, int memberId)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new PromoteOrDemoteGroupMemberCommand(ownerId, groupChatId, memberId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{groupChatId}/members/{memberId}/ban")]
    public async Task<IActionResult> BanOrUnbanGroupMemberAsync(int groupChatId, int memberId)
    {
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new BanOrUnbanGroupMemberCommand(adminId, groupChatId, memberId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }
}
