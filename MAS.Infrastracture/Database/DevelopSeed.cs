using MAS.Core.Entities.ChatEntities;
using MAS.Core.Entities.JoinEntities;
using MAS.Core.Entities.MessageEntities;
using MAS.Core.Entities.UserEntities;
using MAS.Core.Enums;
using System.Security.Cryptography;

namespace MAS.Infrastracture.Database;

/*
 * how many easter eggs you can find?
 */
public class DevelopSeed
{
    public static async Task Seed(EfDbContext dbContext)
    {
        using var hmac = new HMACSHA512();

        if (!dbContext.Users.Any())
        {
            var admin = new User()
            {
                /* ken */
                DisplayName = "Admin",
                Username = "Admin",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("sysadmin")),
                PasswordSalt = hmac.Key,
                Bio = "Behold, this is the Admin.",
                IsVerified = true,
                Sessions = new List<Session> { new() { ClientName = "mMAS", OS = "Plan 9 from Bell Labs" } }
            };
            var chatBot = new User()
            {
                DisplayName = "MAS ChatBot",
                Username = "MASChatBot",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("MASBOT1234")),
                PasswordSalt = hmac.Key,
                Bio = "This bot is connected to a LLM. Feel free to chat.",
                IsBot = true,
                IsVerified = true
            };
            var tester = new User()
            {
                DisplayName = "Tester",    // this is a random user, nothing fancy
                Username = "tester",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                PasswordSalt = hmac.Key,
                Bio = "Behold, this is the Tester.",
                Sessions = new List<Session> { new() { ClientName = "HexMAS", OS = "DuskOS" } }
            };

            var lonelydog = new User()
            {
                DisplayName = "dog",    /* used to test for cases where a message should not be recivied */
                Username = "lonelydog", /* cont'd, i.e: this dog must not be in same group with others   */
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                PasswordSalt = hmac.Key, /* cont'd: or get messages from other private chats             */
                Bio = "I'm just a dog, nobody loves me.",
                Sessions = new List<Session> { new() { ClientName = "Massi", OS = "OpenBSD" } }
            };

            /* bots ahead */
            var vsaeed = new User()
            {
                DisplayName = "virtual saeed",
                Username = "vsaeed",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345678")),
                PasswordSalt = hmac.Key,
                Bio = "mildly bored",
                IsBot = true,
                Sessions = new List<Session> { new() { ClientName = "xmas", OS = "OpenBSD" } }
            };

            var bot = new User()
            {
                DisplayName = "Test Bot",
                Username = "testbot",
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("thisisatoken")),
                PasswordSalt = hmac.Key,
                Bio = "Behold, this the Test bot.",
                IsBot = true
            };
            await dbContext.Users.AddRangeAsync(admin, chatBot, tester, lonelydog, vsaeed);
            await dbContext.SaveChangesAsync();

            var privateChat = new PrivateChat()
            {
                Members = new List<User> { admin, tester },
                Messages = new List<Message>
                {
                    new() { Text = "Welcome to MASsenger!", Sender = admin }
                }
            };
            var chatBotPrivateChat = new PrivateChat()
            {
                Members = new List<User> { admin, chatBot },
            };
            await dbContext.PrivateChats.AddRangeAsync(privateChat, chatBotPrivateChat);
            await dbContext.SaveChangesAsync();

            var groupChat = new GroupChat()
            {
                DisplayName = "Testers Group",
                Groupname = "testersgroup",
                Description = "Behold, this is the Testers group.",
                IsPublic = true,
                Members = new List<GroupChatUser>()
                { new() { Member = admin, Role = GroupChatRole.Owner },
                  new() { Member = tester, Role = GroupChatRole.Admin },
                  new() { Member = vsaeed, Role = GroupChatRole.Member } },
                Messages = new List<Message>()
                {
                    new() { Text = "Testers Group created.", Sender = admin },
                    new() { Text = "Hello World!", Sender = tester },
                    new()
                    {
                        FileName = "textfile",
                        FileSize = 3,
                        FileContent = new FileContent() { Content = new byte[] { 0x6c, 0x6f, 0x6, 0xa, 0x0 } },
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
