using MASsenger.Application.Commands.ChannelChatCommands;
using MASsenger.Application.Dtos.Create;
using MASsenger.Application.Interfaces;
using Moq;

namespace MASsenger.Test.UnitTests.CommandsTests.ChannelChatCommands
{
    public class ChannelChatValidatorTests
    {
        private readonly Mock<IChannelChatRepository> _channelRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private AddChannelChatCommandValidator _validator;
        public ChannelChatValidatorTests() 
        {
            _channelRepositoryMock = new();
            _userRepositoryMock = new();
            _unitOfWorkMock = new();
            _validator = new AddChannelChatCommandValidator();
        }

        [Fact]
        public async Task Validator_Should_Pass_If_Group_Is_Valid()
        {
            var addChannelChatDto = new ChannelChatCreateDto()
            {
                    Description = "Test",
                    Name = "Test",
                    Username = "Test"
            };
            var ownerId = 1;

            var query = new AddChannelChatCommand(addChannelChatDto, ownerId);

            var result = await _validator.ValidateAsync(query);
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validator_Should_Fail_If_Username_Is_Empty()
        {
            var addChannelChatDto = new ChannelChatCreateDto()
            {
                Description = "Test",
                Name = "Test",
                Username = ""
            };
            var ownerId = 1;

            var query = new AddChannelChatCommand(addChannelChatDto, ownerId);

            var result = await _validator.ValidateAsync(query);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validator_Should_Fail_If_Userame_Is_Too_Long()
        {
            var addChannelChatDto = new ChannelChatCreateDto()
            {
                Description = "Test",
                Name = "Test",
                Username = "IWriteThisAsIWeepOnHowPityfulIHaveBecameLestISurviveLongerLikeThis"
            };
            var ownerId = 1;

            var query = new AddChannelChatCommand(addChannelChatDto, ownerId);

            var result = await _validator.ValidateAsync(query);
            Assert.False(result.IsValid);
        }
    }
}