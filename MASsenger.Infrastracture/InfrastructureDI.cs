using MASsenger.Core.Interfaces;
using MASsenger.Core.Options;
using MASsenger.Infrastracture.Data;
using MASsenger.Infrastracture.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MASsenger.Infrastracture
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfratructureDI(this IServiceCollection services)
        {
            services.AddDbContext<MessengerDbContext>((provider, options) =>
            {
                options.UseSqlite(provider.GetRequiredService<IOptionsSnapshot<ConnectionStringOptions>>().Value.DefaultConnection);
            });

            services.AddScoped<DapperDbContext>(provider =>
            {
                return new DapperDbContext(provider.GetRequiredService<IOptionsSnapshot<ConnectionStringOptions>>().Value.DefaultConnection);
            });

            services.AddScoped<IBaseUserRepository, BaseUserRepository>();
            services.AddScoped<IBaseChatRepository, BaseChatRepository>();
            services.AddScoped<IBaseMessageRepository, BaseMessageRepository>();

            return services;
        }
    }
}
