using MAS.Application.Commands.GroupChatCommands;
using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Queries.GroupChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

    [SwaggerOperation(
    Summary = "Get all group chats",
    Description = "Returns the list of all group chats in the system. Accessible only by admins."
    )]
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllGroupChatsAsync()
    {
        var result = await _sender.Send(new GetAllGroupChatsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Get user's group chats",
    Description = "Returns the list of the group chats that the authenticated user is a member of."
    )]
    [HttpGet]
    public async Task<IActionResult> GetAllUserGroupChatsAsync()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new GetAllUserGroupChatsQuery(userId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Get a group chat",
    Description = "Returns the details of a specific group chat."
    )]
    [HttpGet("{groupChatId}")]
    public async Task<IActionResult> GetGroupChatAsync(int groupChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new GetGroupChatQuery(userId, groupChatId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Get group chat members",
    Description = "Returns the list of all members in the specified group chat."
    )]
    [HttpGet("{groupChatId}/members")]
    public async Task<IActionResult> GetGroupChatMembersAsync(int groupChatId)
    {
        var result = await _sender.Send(new GetGroupChatMembersQuery(groupChatId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Create a group chat",
    Description = "Creates a new group chat owned by the authenticated user."
    )]
    [HttpPost]
    public async Task<IActionResult> AddGroupChatAsync(GroupChatAddDto groupChat)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddGroupChatCommand(ownerId, groupChat));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Update a group chat",
    Description = "Updates the details of an existing group chat."
    )]
    [HttpPut]
    public async Task<IActionResult> UpdateGroupChatAsync(GroupChatUpdateDto groupChat)
    {
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new UpdateGroupChatCommand(adminId, groupChat));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Delete a group chat",
    Description = "Deletes the specified group chat owned by the authenticated user."
    )]
    [HttpDelete("{groupChatId}")]
    public async Task<IActionResult> DeleteGroupChatAsync(int groupChatId)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new DeleteGroupChatCommand(ownerId, groupChatId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Join a group chat",
    Description = "Adds the authenticated user to the specified group chat."
    )]
    [HttpPost("{groupChatId}/members/join")]
    public async Task<IActionResult> JoinGroupChatAsync(int groupChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new JoinGroupChatCommand(userId, groupChatId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Leave a group chat",
    Description = "Removes the authenticated user from the specified group chat."
    )]
    [HttpDelete("{groupChatId}/members/leave")]
    public async Task<IActionResult> LeaveGroupChatAsync(int groupChatId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new LeaveGroupChatCommand(userId, groupChatId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Add a group member",
    Description = "Adds the specified user to the specified group chat."
    )]
    [HttpPost("{groupChatId}/members/{memberId}/add")]
    public async Task<IActionResult> AddGroupMemberAsync(int groupChatId, int memberId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddGroupMemberCommand(userId, groupChatId, memberId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Promote or demote a group member",
    Description = "Changes the role of the specified member in the specified group chat."
    )]
    [HttpPut("{groupChatId}/members/{memberId}/promote")]
    public async Task<IActionResult> PromoteOrDemoteGroupMemberAsync(int groupChatId, int memberId)
    {
        var ownerId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new PromoteOrDemoteGroupMemberCommand(ownerId, groupChatId, memberId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Ban or unban a group member",
    Description = "Bans or unbans the specified member in the specified group chat."
    )]
    [HttpPut("{groupChatId}/members/{memberId}/ban")]
    public async Task<IActionResult> BanOrUnbanGroupMemberAsync(int groupChatId, int memberId)
    {
        var adminId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new BanOrUnbanGroupMemberCommand(adminId, groupChatId, memberId));
        return StatusCode(result.StatusCode, result);
    }
}
