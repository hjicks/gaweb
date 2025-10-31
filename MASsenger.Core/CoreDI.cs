using MASsenger.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Core
{
    public static class CoreDI
    {
        public static IServiceCollection AddCoreDI(this IServiceCollection services, IConfiguration configurtion)
        {
            services.Configure<ConnectionStringOptions>(configurtion.GetSection(ConnectionStringOptions.SectionName));
            return services;
        }
    }
}
