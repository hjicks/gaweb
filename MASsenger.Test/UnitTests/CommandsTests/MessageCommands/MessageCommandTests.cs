using MASsenger.Application.Commands.ChannelChatCommands;
using MASsenger.Application.Commands.MessageCommands;
using MASsenger.Application.Dtos.MessageDtos;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Results;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Core.Entities.UserEntities;
using MASsenger.Core.Enums;
using Moq;

namespace MASsenger.Test.UnitTests.CommandsTests.MessageCommands
{
    public class MessageCommandTests
    {
        private readonly Mock<IMessageRepository> _messageRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public MessageCommandTests()
        {
            _messageRepositoryMock = new();
            _userRepositoryMock = new();
            _unitOfWorkMock = new();
        }

        [Fact]
        public async Task Handle_should_work_updating()
        {
            var user = new User()
            {
                Username = "TestUser",
            };

            var umsg = new MessageUpdateDto()
            {
                Id = 1,
                Text = "Hello"
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync(user);

            _messageRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Int32>()))
                .ReturnsAsync(new Message()
                {
                    Text = "Hello",
                    Sender = user
                });

            var query = new UpdateMessageCommand(user.Id, umsg);
            var handler = new UpdateMessageCommandHandler(_messageRepositoryMock.Object, _unitOfWorkMock.Object);
            Result result = await handler.Handle(query, default);

            Assert.True(result.Ok);
        }
    }
}
