using MASsenger.Core.Entities;
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
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin")),
                    PasswordSalt = hmac.Key,
                    Description = "Behold, this is the Admin.",
                    IsVerified = true
                };
                var tester = new User()
                {
                    Name = "Tester",    // this is a random user, nothing fancy
                    Username = "tester",
                    PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("12345")),
                    PasswordSalt = hmac.Key,
                    Description = "Behold, this is the Tester."
                };
                await dbContext.Users.AddRangeAsync(admin, tester);

                var bot = new Bot()
                {
                    Name = "Testers Bot",
                    Username = "testersbot",
                    Token = "thisisatoken",
                    Description = "Behold, this the Testers bot.",
                    Owner = tester
                };
                await dbContext.Bots.AddAsync(bot);

                var adminSession = new Session()
                {
                    User = admin
                };
                var testerSession = new Session()
                {
                    User = tester
                };
                await dbContext.Sessions.AddRangeAsync(adminSession, testerSession);

                var privateChat = new PrivateChat()
                {
                    Starter = admin,
                    Receiver = tester
                };
                await dbContext.PrivateChats.AddAsync(privateChat);

                var channel = new ChannelChat()
                {
                    Name = "Testers Channel",
                    Username = "testerschannel",
                    Description = "Behold, this is the Testers channel.",
                    Owner = tester,
                    Admins = new List<BaseUser>() { tester },
                    Members = new List<BaseUser>() { tester }
                };
                await dbContext.ChannelChats.AddAsync(channel);

                var systemMessage = new SystemMessage()
                {
                    Text = "Testers Channel created.",
                    Destination = channel
                };
                await dbContext.SystemMessages.AddAsync(systemMessage);

                var channelMessage = new Message()
                {
                    Text = "Hello World!",
                    Sender = tester,
                    Destination = channel
                };
                var privateMessage = new Message()
                {
                    Text = "Welcome to MASsenger!",
                    Sender = admin,
                    Destination = privateChat
                };
                await dbContext.Messages.AddRangeAsync(channelMessage, privateMessage);


                await dbContext.SaveChangesAsync();
            }
        }
    }
}
