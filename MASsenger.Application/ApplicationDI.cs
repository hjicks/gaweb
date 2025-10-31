using Microsoft.Extensions.DependencyInjection;

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
            return services;
        }
    }
}
