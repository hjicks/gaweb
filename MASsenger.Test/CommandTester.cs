using MASsenger.Api;
using MASsenger.Application;
using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Queries.UserQueries;
using MASsenger.Core;
using MASsenger.Infrastracture;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MASsenger.Test
{
    [TestClass]
    public sealed class CommandsTester
    {
        private readonly ISender _sender;

        public CommandsTester()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Configuration.AddJsonFile("appsettings.Development.json");
            builder.Services.AddCoreDI(builder.Configuration)
                .AddApplicationDI()
                .AddInfratructureDI()
                .AddApiDI(builder.Configuration);
            var serviceProvider = builder.Services.BuildServiceProvider();
            _sender = serviceProvider.GetRequiredService<ISender>();
        }

        [TestMethod]
        public void  AdminLoginTest()
        {
        UserLoginDto userCred = new UserLoginDto();
        userCred.Username = "Admin";
        userCred.Password = "admin";

        Assert.AreNotEqual("error", _sender.Send(new LoginUserQuery(userCred)).Result);
        }
    }
}
