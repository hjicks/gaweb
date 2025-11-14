using MASsenger.Application.Commands.UserCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Dtos.Read;
using MASsenger.Application.Dtos.Update;
using MASsenger.Application.Queries.UserQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASsenger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly ISender _sender;
        public UserController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userCred)
        {
            if (await _sender.Send(new LoginUserQuery(userCred)) == "error") return BadRequest("Something went wrong while logging in.");
            return Ok(await _sender.Send(new LoginUserQuery(userCred)));
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserReadDto>))]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _sender.Send(new GetAllUsersQuery()));
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> AddUserAsync([FromBody] UserCreateDto user)
        {
            if (await _sender.Send(new AddUserCommand(user)) == Core.Enums.TransactionResultType.Done) return Ok("User added successfully.");
            return BadRequest("Something went wrong while saving the user.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UserUpdateDto user)
        {
            if (await _sender.Send(new UpdateUserCommand(user)) == Core.Enums.TransactionResultType.Done) return Ok("User updated successfully.");
            else if (await _sender.Send(new UpdateUserCommand(user)) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while updating the user.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAsync()
        {
            if (await _sender.Send(new DeleteUserCommand()) == Core.Enums.TransactionResultType.Done) return Ok("User deleted successfully.");
            else if (await _sender.Send(new DeleteUserCommand()) == Core.Enums.TransactionResultType.ForeignKeyNotFound) return Ok("Invalid user Id.");
            return BadRequest("Something went wrong while deleting the user.");
        }
    }
}
