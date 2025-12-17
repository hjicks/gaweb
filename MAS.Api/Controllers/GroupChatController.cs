using MAS.Application.Commands.GroupChatCommands;
using MAS.Application.Dtos.GroupChatDtos;
using MAS.Application.Queries.GroupChatQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace MAS.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "User,Bot")]
public class GroupChatController : BaseController
{
    public GroupChatController(ISender sender) : base(sender)
    {

    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<GroupChatGetDto>))]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> GetAllChannelChats()
    {
        return Ok(await _sender.Send(new GetAllGroupChatsQuery()));
    }

    [HttpPost]
    public async Task<IActionResult> AddChannelChatAsync([FromBody] PublicGroupChatAddDto groupChat)
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
    public async Task<IActionResult> UpdateChannelChatAsync(PublicGroupChatUpdateDto groupChat)
    {
        if (await _sender.Send(new UpdateGroupChatCommand(groupChat)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat updated successfully.");
        else if (await _sender.Send(new UpdateGroupChatCommand(groupChat)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid channelChat Id.");
        return BadRequest("Something went wrong while updating the channelChat.");
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteChannelChatAsync(Int32 groupChatId)
    {
        if (await _sender.Send(new DeleteGroupChatCommand(groupChatId)) == Core.Enums.TransactionResultType.Done) return Ok("ChannelChat deleted successfully.");
        else if (await _sender.Send(new DeleteGroupChatCommand(groupChatId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid channelChat Id.");
        return BadRequest("Something went wrong while deleting the channelChat.");
    }
}
