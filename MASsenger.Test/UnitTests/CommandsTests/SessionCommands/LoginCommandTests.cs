using MASsenger.Application.Commands.SessionCommands;
using MASsenger.Application.Dtos.UserDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MASsenger.Core.Entities.UserEntities;
using Moq;
using System.Security.Cryptography;

namespace MASsenger.Test.UnitTests.CommandsTests.SessionCommands
{
    public class LoginCommandTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private LoginCommandValidator _validator;
        public LoginCommandTests() 
        {
            _userRepositoryMock = new();
            _sessionRepositoryMock = new();
            _unitOfWorkMock = new();
            _jwtServiceMock = new();
            _validator = new LoginCommandValidator();
        }

        [Fact]
        public async Task Validator_Should_Pass_If_Password_Is_Valid()
        {
            // Arrange
            var userLoginDto = new UserLoginDto()
            {
                Username = "Tester",
                Password = "12345678"
            };
            var query = new LoginCommand(userLoginDto);

            var result = await _validator.ValidateAsync(query);
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validator_Should_Fail_If_Password_Is_Empty()
        {
            // Arrange
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
            // Arrange
            var userLoginDto = new UserLoginDto()
            {
                Username = "Tester",
                Password = "1234"
            };
            var query = new LoginCommand(userLoginDto);

            var result = await _validator.ValidateAsync(query);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Handle_Should_NotSucceed_WhenPasswordIsWrong()
        {
            // Arrange
            var userLoginDto = new UserLoginDto()
            {
                Username = "Tester",
                Password = "wrongPass"
            };
            var query = new LoginCommand(userLoginDto);

            var salt = new byte[] { 1, 2, 3, 4 };
            using var hmac = new HMACSHA512(salt);
            _userRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("truePass")),
                    PasswordSalt = salt
                });

            var handler = new LoginCommandHandler(
                _userRepositoryMock.Object, _sessionRepositoryMock.Object,
                _unitOfWorkMock.Object, _jwtServiceMock.Object);

            // Act
            Result result = await handler.Handle(query, default);

            // Assert
            Assert.False(result.Ok);
        }
    }
}