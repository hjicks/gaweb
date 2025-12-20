using MAS.Application.Interfaces;
using MAS.Core.Options;
using MAS.Infrastracture.Database;
using MAS.Infrastracture.Repositories;
using MAS.Infrastracture.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MAS.Infrastracture
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
            services.AddScoped<IBaseChatRepository, BaseChatRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupChatRepository, GroupChatRepository>();
            services.AddScoped<IPrivateChatRepository, PrivateChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();

            return services;
        }
    }
}
