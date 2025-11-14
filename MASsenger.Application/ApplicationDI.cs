using MASsenger.Application.Interfaces;
using MASsenger.Core.Options;
using MASsenger.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MASsenger.Application
{
    public static class ApplicationDI
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ApplicationDI).Assembly);
            });

            services.AddScoped<IJwtService, JwtService>(provider =>
            {
                return new JwtService(provider.GetRequiredService<IOptionsSnapshot<JwtOptions>>().Value.Key);
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
