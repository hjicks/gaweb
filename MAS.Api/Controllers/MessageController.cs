using MAS.Application.Commands.MessageCommands;
using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Queries.MessageQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MAS.Api.Controllers;

[Route("api/messages")]
[ApiController]
[Authorize(Roles = "User")]
public class MessageController : BaseController
{
    public MessageController(ISender sender) : base(sender)
    {

    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllMessagesAsync()
    {
        var result = await _sender.Send(new GetAllMessagesQuery());
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("last/{chatId}")]
    public async Task<IActionResult> GetChatLastMessageAsync(int chatId)
    {
        var result = await _sender.Send(new GetChatLastMessageQuery(chatId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{messageId}")]
    public async Task<IActionResult> GetChatLastMessagesAsync(int messageId)
    {
        var result = await _sender.Send(new GetChatLastMessagesQuery(messageId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> AddMessageAsync([FromBody] MessageAddDto message)
    {
        var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddMessageCommand(senderId, message));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessageAsync(int messageId)
    {
        var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new DeleteMessageCommand(senderId, messageId));
        if (result.Ok)
        {
            return StatusCode(result.StatusCode, result);
        }
        return StatusCode(result.StatusCode, result);
    }
}
