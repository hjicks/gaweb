using MASsenger.Core.Interfaces;
using MASsenger.Infrastracture.Data;
using MASsenger.Infrastracture.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASsenger.Infrastracture
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfratructureDI(this IServiceCollection services)
        {
            services.AddDbContext<MessengerDbContext>(options =>
            {
                options.UseSqlite("Data source=C:\\Users\\mahdi\\source\\repos\\gaweb\\MASsenger.Infrastracture\\Database\\MASsengerDB.db");
            });

            services.AddScoped<IBaseUserRepository, BaseUserRepository>();
            services.AddScoped<IBaseChatRepository, BaseChatRepository>();
            services.AddScoped<IBaseMessageRepository, BaseMessageRepository>();

            return services;
        }
    }
}
