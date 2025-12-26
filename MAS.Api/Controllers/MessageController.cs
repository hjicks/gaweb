using MAS.Application.Commands.MessageCommands;
using MAS.Application.Dtos.MessageDtos;
using MAS.Application.Queries.MessageQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
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

    [SwaggerOperation(
    Summary = "Get all messages",
    Description = "Returns the list of all messages in the system. Accessible only by admins."
    )]
    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllMessagesAsync()
    {
        var result = await _sender.Send(new GetAllMessagesQuery());
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Get last message of a chat",
    Description = "Returns the last message of the specified chat."
    )]
    [HttpGet("last/{chatId}")]
    public async Task<IActionResult> GetChatLastMessageAsync(int chatId)
    {
        var result = await _sender.Send(new GetChatLastMessageQuery(chatId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Get previous messages",
    Description = "Returns messages that were sent before the specified message."
    )]
    [HttpGet("{messageId}")]
    public async Task<IActionResult> GetChatLastMessagesAsync(int messageId)
    {
        var result = await _sender.Send(new GetChatLastMessagesQuery(messageId));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Add a message",
    Description = "Creates a new message in a chat."
    )]
    [HttpPost]
    public async Task<IActionResult> AddMessageAsync(MessageAddDto message)
    {
        var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new AddMessageCommand(senderId, message));
        return StatusCode(result.StatusCode, result);
    }

    [SwaggerOperation(
    Summary = "Delete a message",
    Description = "Deletes the specified message."
    )]
    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessageAsync(int messageId)
    {
        var senderId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _sender.Send(new DeleteMessageCommand(senderId, messageId));
        return StatusCode(result.StatusCode, result);
    }
}
