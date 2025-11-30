namespace MASsenger.Test.UnitTests.CommandsTests.ChannelChatCommands
{
    public class ChannelChatCommandTests
    {
        // todo, we need more checks in command
        //[Fact]
        //public async Task Handle_Should_NotSucceed_WhenUsernameExists()
        //{
        //    var channelChatCreateDto = new ChannelChatCreateDto()
        //    {
        //        Description = "Test",
        //        Name = "Test",
        //        Username = "Test"
        //    };
        //    var ownerId = 1;
        //    var query = new AddChannelChatCommand(channelChatCreateDto, ownerId);

        //    _userRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Int32>()))
        //        .ReturnsAsync(new User()
        //        {
        //            Username = "TestUser",
        //        });
        //    _channelRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Int32>()))
        //        .ReturnsAsync(new ChannelChat()
        //        {
        //            Name = "Test2",
        //            Description =  "Test",
        //            Username = "Test",
        //        });

        //    var handler = new AddChannelChatCommandHandler(_channelRepositoryMock.Object,
        //        _userRepositoryMock.Object,
        //        _unitOfWorkMock.Object);

        //    TransactionResultType result = await handler.Handle(query, default);

        //    Assert.NotEqual(result, TransactionResultType.Done);
        //}
    }
}
