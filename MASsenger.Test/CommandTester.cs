using MASsenger.Api;
using MASsenger.Application;
using MASsenger.Application.Dtos.Login;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Queries.UserQueries;
using MASsenger.Application.Services;
using MASsenger.Core.Options;
using MASsenger.Infrastracture;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories;
using MASsenger.Infrastracture.Repositories.Base;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            builder.Services.AddApiDI(builder.Configuration);
            var serviceProvider = builder.Services.BuildServiceProvider();
            _sender = serviceProvider.GetRequiredService<ISender>();
        }

        [TestMethod]
        public void  AdminLoginTest()
        {
        UserLoginDto userCred = new UserLoginDto();
        userCred.Username = "Admin";
        userCred.Password = "admin";

         Assert.AreNotEqual(_sender.Send(new LoginUserQuery(userCred)).Result, "error");
    }
    }
}
