using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MAS.Infrastracture.Database
{
    public class DevelopSeed
    {
        public static async Task Seed(EfDbContext dbContext)
        {
            using var hmac = new HMACSHA512();

            if (!dbContext.Users.Any())
            {
                var admin = new User()
                {
                    DisplayName = "Admin",
                    Username = "Admin",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("sysadmin")),
                    PasswordSalt = hmac.Key,
                    Bio = "Behold, this is the Admin.",
                    IsVerified = true
                };
                var tester = new User()
                {
                    DisplayName = "Tester",    // this is a random user, nothing fancy
                    Username = "tester",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                    PasswordSalt = hmac.Key,
                    Bio = "Behold, this is the Tester."
                };
                var bot = new User()
                {
                    DisplayName = "Testers Bot",
                    Username = "testersbot",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("thisisatoken")),
                    PasswordSalt = hmac.Key,
                    Bio = "Behold, this the Testers bot."
                };
                await dbContext.Users.AddRangeAsync(admin, tester, bot);
                await dbContext.SaveChangesAsync();

                var dbAdmin = await dbContext.Users.Where(u => u.Username == admin.Username).SingleAsync();
                var dbTester = await dbContext.Users.Where(u => u.Username == tester.Username).SingleAsync();

                var adminSession = new Session()
                {
                    UserId = dbAdmin.Id
                };
                var testerSession = new Session()
                {
                    UserId = dbTester.Id
                };
                await dbContext.Sessions.AddRangeAsync(adminSession, testerSession);
                await dbContext.SaveChangesAsync();

                var privateChat = new PrivateChat()
                {
                    Members = new List<User> { dbAdmin, dbTester }
                };
                await dbContext.PrivateChats.AddAsync(privateChat);
                await dbContext.SaveChangesAsync();

                var group = new GroupChat()
                {
                    DisplayName = "Testers Channel",
                    Groupname = "testerschannel",
                    Description = "Behold, this is the Testers channel.",
                    //OwnerId = dbTester.Id,
                    //Admins = new List<BaseUser>() { dbTester },
                    Members = new List<User>() { dbTester }
                };
                await dbContext.GroupChats.AddAsync(group);
                await dbContext.SaveChangesAsync();

                //var dbPrivateChat = await dbContext.PrivateChats.Where(c => c.StarterId == privateChat.StarterId).SingleAsync();
                //var dbChannel = await dbContext.GroupChats.Where(c => c.Username == group.Username).SingleAsync();

                //var systemMessage = new SystemMessage()
                //{
                //    Text = "Testers Channel created.",
                //    DestinationId = dbChannel.Id
                //};
                //await dbContext.SystemMessages.AddAsync(systemMessage);
                //await dbContext.SaveChangesAsync();

                //var groupMessage = new Message()
                //{
                //    Text = "Hello World!",
                //    SenderId = dbTester.Id,
                //    DestinationId = dbChannel.Id
                //};
                //var privateMessage = new Message()
                //{
                //    Text = "Welcome to MASsenger!",
                //    SenderId = dbAdmin.Id,
                //    DestinationId = dbPrivateChat.Id
                //};
                //await dbContext.Messages.AddRangeAsync(groupMessage, privateMessage);
                //await dbContext.SaveChangesAsync();
            }
        }
    }
}
