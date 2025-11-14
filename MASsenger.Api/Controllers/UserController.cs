using MASsenger.Application.Commands.UserCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _sender.Send(new GetAllUsersQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto user)
        {
            if (await _sender.Send(new AddUserCommand(user)) == Core.Enums.TransactionResultType.Done) return Ok("User added successfully.");
            return BadRequest("Something went wrong while saving the user.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserPasswdAsync(UserPasswdUpdateDto user)
        {
            if (await _sender.Send(new UpdateUserPasswdCommand(user)) == Core.Enums.TransactionResultType.Done) return Ok("Password updated successfully.");
            else if (await _sender.Send(new UpdateUserPasswdCommand(user)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while updating the user.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
        {
            if (await _sender.Send(new UpdateUserCommand(user)) == Core.Enums.TransactionResultType.Done) return Ok("User updated successfully.");
            else if (await _sender.Send(new UpdateUserCommand(user)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while updating the user.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync(Int32 userId)
        {
            if (await _sender.Send(new DeleteUserCommand(userId)) == Core.Enums.TransactionResultType.Done) return Ok("User deleted successfully.");
            else if (await _sender.Send(new DeleteUserCommand(userId)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while deleting the user.");
        }
    }
}
