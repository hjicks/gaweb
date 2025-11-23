using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Queries.UserQueries;
using MASsenger.Core.Entities;
using Moq;
using System.Security.Cryptography;

namespace MASsenger.Test.UnitTests.QueriesTests.UserQueries
{
    public class LoginUserQueryTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        public LoginUserQueryTests() 
        {
            _userRepositoryMock = new();
            _jwtServiceMock = new();
        }

        [Fact]
        public async Task Handle_Should_ReturnError_WhenPasswordIsWrong()
        {
            // Arrange
            var userLoginDto = new UserLoginDto()
            {
                Username = "Tester",
                Password = "wrongPass"
            };
            var query = new LoginUserQuery(userLoginDto);

            var salt = new byte[] { 1, 2, 3, 4 };
            using var hmac = new HMACSHA512(salt);
            _userRepositoryMock.Setup(x => x.GetByUsernameAsync(It.IsAny<string>()))
                .ReturnsAsync(new User()
                {
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("truePass")),
                    PasswordSalt = salt
                });

            var handler = new LoginUserQueryHandler(
                _userRepositoryMock.Object, _jwtServiceMock.Object);

            // Act
            string result = await handler.Handle(query, default);

            // Assert
            Assert.Equal("error", result);
        }
    }
}