using MASsenger.Application.Commands.UserCommands;
using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Responses;
using MASsenger.Core.Entities.UserEntities;
using Moq;
using System.Security.Cryptography;

namespace MASsenger.Test.UnitTests.CommandsTests.UserCommands
{
    public class LoginUserCommandTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ISessionRepository> _sessionRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        public LoginUserCommandTests() 
        {
            _userRepositoryMock = new();
            _sessionRepositoryMock = new();
            _unitOfWorkMock = new();
            _jwtServiceMock = new();
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
            var query = new LoginUserCommand(userLoginDto);

            var salt = new byte[] { 1, 2, 3, 4 };
            using var hmac = new HMACSHA512(salt);
            _userRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("truePass")),
                    PasswordSalt = salt
                });

            var handler = new LoginUserCommandHandler(
                _userRepositoryMock.Object, _sessionRepositoryMock.Object,
                _unitOfWorkMock.Object, _jwtServiceMock.Object);

            // Act
            Result<TokensResponse> result = await handler.Handle(query, default);

            // Assert
            Assert.False(result.Success);
        }
    }
}