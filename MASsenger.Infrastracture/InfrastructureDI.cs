using MASsenger.Application.Interfaces;
using MASsenger.Core.Options;
using MASsenger.Infrastracture.Database;
using MASsenger.Infrastracture.Repositories;
using MASsenger.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MASsenger.Infrastracture
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfratructureDI(this IServiceCollection services)
        {
            services.AddDbContext<EfDbContext>((provider, options) =>
            {
                options.UseSqlite(provider.GetRequiredService<IOptions<ConnectionStringOptions>>().Value.DefaultConnection);
            });

            services.AddScoped<DapperDbContext>(provider =>
            {
                return new DapperDbContext(provider.GetRequiredService<IOptions<ConnectionStringOptions>>().Value.DefaultConnection);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBotRepository, BotRepository>();
            services.AddScoped<IChannelChatRepository, ChannelChatRepository>();
            services.AddScoped<IPrivateChatRepository, PrivateChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ISystemMessageRepository, SystemMessageRepository>();

            return services;
        }
    }
}
