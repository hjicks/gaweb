using MAS.Application.Commands.GroupChatCommands;
using MAS.Application.Dtos.GroupChatDtos;

namespace MAS.Test.UnitTests.CommandsTests.GroupChatCommands
{
    public class GroupChatValidatorTests
    {
        private AddGroupChatCommandValidator _validator;
        public GroupChatValidatorTests() 
        {
            _validator = new AddGroupChatCommandValidator();
        }

        [Fact]
        public async Task Validator_Should_Pass_If_Group_Is_Valid()
        {
            var addGroupChatDto = new GroupChatAddDto()
            {
                    Description = "Test",
                    DisplayName = "Test",
                    Groupname = "TestGroupname"
            };
            var ownerId = 1;

            var query = new AddGroupChatCommand(ownerId, addGroupChatDto);

            var result = await _validator.ValidateAsync(query);
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validator_Should_Fail_If_Username_Is_Empty()
        {
            var addGroupChatDto = new GroupChatAddDto()
            {
                Description = "Test",
                DisplayName = "Test",
                Groupname = ""
            };
            var ownerId = 1;

            var query = new AddGroupChatCommand(ownerId, addGroupChatDto);

            var result = await _validator.ValidateAsync(query);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validator_Should_Fail_If_Userame_Is_Too_Long()
        {
            var addGroupChatDto = new GroupChatAddDto()
            {
                Description = "Test",
                DisplayName = "Test",
                Groupname = "IWriteThisAsIWeepOnHowPityfulIHaveBecameLestISurviveLongerLikeThis"
            };
            var ownerId = 1;

            var query = new AddGroupChatCommand(ownerId, addGroupChatDto);

            var result = await _validator.ValidateAsync(query);
            Assert.False(result.IsValid);
        }
    }
}