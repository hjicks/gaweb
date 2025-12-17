using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace MAS.Infrastracture.Database;

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
                IsVerified = true,
                Sessions = new List<Session> { new() }
            };
            var tester = new User()
            {
                DisplayName = "Tester",    // this is a random user, nothing fancy
                Username = "tester",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                PasswordSalt = hmac.Key,
                Bio = "Behold, this is the Tester.",
                Sessions = new List<Session> { new() }
            };
            var bot = new User()
            {
                DisplayName = "Test Bot",
                Username = "testbot",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("thisisatoken")),
                PasswordSalt = hmac.Key,
                Bio = "Behold, this the Test bot.",
            };
            await dbContext.Users.AddRangeAsync(admin, tester, bot);
            await dbContext.SaveChangesAsync();

            var dbAdmin = await dbContext.Users.Where(u => u.Username == admin.Username).SingleAsync();
            var dbTester = await dbContext.Users.Where(u => u.Username == tester.Username).SingleAsync();

            var privateChat = new PrivateChat()
            {
                Members = new List<User> { dbAdmin, dbTester },
                Messages = new List<Message>
                {
                    new() { Text = "Welcome to MASsenger!", Sender = dbAdmin }
                }
            };
            await dbContext.PrivateChats.AddAsync(privateChat);
            await dbContext.SaveChangesAsync();

            var groupChat = new GroupChat()
            {
                DisplayName = "Testers Group",
                Groupname = "testersgroup",
                Description = "Behold, this is the Testers group.",
                IsPublic = true,
                Members = new List<GroupChatUser>()
                { new() { Member = dbTester, Role = GroupChatRole.Owner } },
                Messages = new List<Message>()
                {
                    new() { Text = "Testers Group created.", Sender = dbAdmin },
                    new() { Text = "Hello World!", Sender = dbTester }
                }
            };
            await dbContext.GroupChats.AddAsync(groupChat);
            await dbContext.SaveChangesAsync();

            var adminActiveSession = await dbContext.Sessions.Where(s => s.UserId == dbAdmin.Id && s.IsRevoked == false).SingleAsync();
            adminActiveSession.IsRevoked = true;
            adminActiveSession.RevokedAt = DateTime.UtcNow;

            var testerActiveSession = await dbContext.Sessions.Where(s => s.UserId == dbTester.Id && s.IsRevoked == false).SingleAsync();
            testerActiveSession.IsRevoked = true;
            testerActiveSession.RevokedAt = DateTime.UtcNow;

            dbContext.Sessions.Update(adminActiveSession);
            dbContext.Sessions.Update(testerActiveSession);
            await dbContext.SaveChangesAsync();
        }
    }
}
