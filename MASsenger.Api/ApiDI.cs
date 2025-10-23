using MASsenger.Application;
using MASsenger.Core;
using MASsenger.Infrastracture;

namespace MASsenger.Api
{
    public static class ApiDI
    {
        public static IServiceCollection AddApiDI(this IServiceCollection services)
        {
            services.AddInfratructureDI()
                      .AddApplicationDI();
            return services;
        }
    }
}
