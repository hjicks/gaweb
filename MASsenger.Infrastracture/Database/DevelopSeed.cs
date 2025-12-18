using MASsenger.Core.Entities.ChatEntities;
using MASsenger.Core.Entities.MessageEntities;
using MASsenger.Core.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MASsenger.Infrastracture.Database
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
                    Name = "Admin",
                    Username = "Admin",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("sysadmin")),
                    PasswordSalt = hmac.Key,
                    Description = "Behold, this is the Admin.",
                    IsVerified = true
                };
                var tester = new User()
                {
                    Name = "Tester",    // this is a random user, nothing fancy
                    Username = "tester",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                    PasswordSalt = hmac.Key,
                    Description = "Behold, this is the Tester."
                };

                var tester2 = new User()
                {
                    Name = "Tester2",
                    Username = "tester2",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                    PasswordSalt = hmac.Key,
                    Description = "Tester II, for testing cases where someone shouldn't get notified on messages that dont relate to them"
                };

                var tester3 = new User()
                {
                    Name = "Tester3",
                    Username = "tester3",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                    PasswordSalt = hmac.Key,
                    Description = "Tester III for testing group's functionality"
                };

                await dbContext.Users.AddRangeAsync(admin, tester, tester2, tester3);
                await dbContext.SaveChangesAsync();

                var dbAdmin = await dbContext.Users.Where(u => u.Username == admin.Username).SingleAsync();
                var dbTester = await dbContext.Users.Where(u => u.Username == tester.Username).SingleAsync();
                var dbTester2 = await dbContext.Users.Where(u => u.Username == tester.Username).SingleAsync();
                var dbTester3 = await dbContext.Users.Where(u => u.Username == tester.Username).SingleAsync();

                var bot = new Bot()
                {
                    Name = "Testers Bot",
                    Username = "testersbot",
                    Token = "thisisatoken",
                    Description = "Behold, this the Testers bot.",
                    OwnerId = dbTester.Id
                };
                await dbContext.Bots.AddAsync(bot);
                await dbContext.SaveChangesAsync();

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
                    StarterId = dbAdmin.Id,
                    ReceiverId = dbTester.Id
                };
                await dbContext.PrivateChats.AddAsync(privateChat);
                await dbContext.SaveChangesAsync();

                var channel = new ChannelChat()
                {
                    Name = "Testers Channel",
                    Username = "testerschannel",
                    Description = "Behold, this is the Testers channel.",
                    OwnerId = dbTester.Id,
                    Admins = new List<BaseUser>() { dbTester },
                    Members = new List<BaseUser>() { dbTester, dbAdmin, dbTester3 }
                };
                await dbContext.ChannelChats.AddAsync(channel);
                await dbContext.SaveChangesAsync();

                var dbPrivateChat = await dbContext.PrivateChats.Where(c => c.StarterId == privateChat.StarterId).SingleAsync();
                var dbChannel = await dbContext.ChannelChats.Where(c => c.Username == channel.Username).SingleAsync();

                var systemMessage = new SystemMessage()
                {
                    Text = "Testers Channel created.",
                    DestinationId = dbChannel.Id
                };
                await dbContext.SystemMessages.AddAsync(systemMessage);
                await dbContext.SaveChangesAsync();

                var channelMessage = new Message()
                {
                    Text = "Hello World!",
                    SenderId = dbTester.Id,
                    DestinationId = dbChannel.Id
                };
                var privateMessage = new Message()
                {
                    Text = "Welcome to MASsenger!",
                    SenderId = dbAdmin.Id,
                    DestinationId = dbPrivateChat.Id
                };
                await dbContext.Messages.AddRangeAsync(channelMessage, privateMessage);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
