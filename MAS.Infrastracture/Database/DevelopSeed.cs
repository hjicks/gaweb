using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
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
                Sessions = new List<Session> { new() { ClientName = "test client", OS = "test OS" } }
            };
            var tester = new User()
            {
                DisplayName = "Tester",    // this is a random user, nothing fancy
                Username = "tester",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                PasswordSalt = hmac.Key,
                Bio = "Behold, this is the Tester.",
                Sessions = new List<Session> { new() { ClientName = "test client", OS = "test OS" } }
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

            var privateChat = new PrivateChat()
            {
                Members = new List<User> { admin, tester },
                Messages = new List<Message>
                {
                    new() { Text = "Welcome to MASsenger!", Sender = admin }
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
                { new() { Member = tester, Role = GroupChatRole.Owner } },
                Messages = new List<Message>()
                {
                    new() { Text = "Testers Group created.", Sender = admin },
                    new() { Text = "Hello World!", Sender = tester },
                    new()
                    {
                        FileName = "textfile",
                        FileSize = 3,
                        FileContent = new FileContent() { Content = new byte[] { 72, 105, 33 } },
                        FileContentType = "text/plain",
                        Sender = tester
                    }
                }
            };
            await dbContext.GroupChats.AddAsync(groupChat);
            await dbContext.SaveChangesAsync();

            // revoke admin and tester active session
            var adminActiveSession = admin.Sessions.Where(s => s.IsRevoked == false).Single();
            adminActiveSession.IsRevoked = true;
            adminActiveSession.RevokedAt = DateTime.UtcNow;
            var testerActiveSession = tester.Sessions.Where(s => s.IsRevoked == false).Single();
            testerActiveSession.IsRevoked = true;
            testerActiveSession.RevokedAt = DateTime.UtcNow;
            dbContext.Sessions.UpdateRange(adminActiveSession, testerActiveSession);
            await dbContext.SaveChangesAsync();
        }
    }
}
