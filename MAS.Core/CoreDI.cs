using MAS.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MAS.Core
{
    public static class CoreDI
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection services, IConfiguration configurtion)
        {
            services.Configure<ConnectionStringOptions>(configurtion.GetSection(ConnectionStringOptions.SectionName));
            services.Configure<JwtOptions>(configurtion.GetSection(JwtOptions.SectionName));
            return services;
        }
    }
}
