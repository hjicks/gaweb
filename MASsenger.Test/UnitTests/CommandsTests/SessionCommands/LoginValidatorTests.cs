using MASsenger.Application.Commands.SessionCommands;
using MASsenger.Application.Dtos.Login;

namespace MASsenger.Test.UnitTests.CommandsTests.SessionCommands
{
    public class LoginValidatorTests
    {
        private LoginCommandValidator _validator;
        public LoginValidatorTests() 
        {
            _validator = new LoginCommandValidator();
        }

        [Fact]
        public async Task Validator_Should_Pass_If_Password_Is_Valid()
        {
            var userLoginDto = new UserLoginDto()
            {
                Username = "Tester",
                Password = "12345678",
            };
            var query = new LoginCommand(userLoginDto);

            var result = await _validator.ValidateAsync(query);
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validator_Should_Fail_If_Password_Is_Empty()
        {
            var userLoginDto = new UserLoginDto()
            {
                Username = "Tester",
                Password = ""
            };
            var query = new LoginCommand(userLoginDto);

            var result = await _validator.ValidateAsync(query);
            Assert.False(result.IsValid);
        }


        [Fact]
        public async Task Validator_Should_Fail_If_Password_Is_Short()
        {
            var userLoginDto = new UserLoginDto()
            {
                Username = "Tester",
                Password = "1234"
            };
            var query = new LoginCommand(userLoginDto);

            var result = await _validator.ValidateAsync(query);
            Assert.False(result.IsValid);
        }
    }
}