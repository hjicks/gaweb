using FluentValidation;
using MASsenger.Application.Interfaces;
using MASsenger.Application.Services;
using MASsenger.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

            services.AddValidatorsFromAssembly(typeof(ApplicationDI).Assembly);

            services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(provider =>
            {
                return new ConfigureNamedOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    var jwtOptions = provider.GetRequiredService<IOptions<JwtOptions>>().Value;
                    if (string.IsNullOrWhiteSpace(jwtOptions.Key))
                        throw new InvalidOperationException("Jwt key is not configured.");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            services.AddScoped<IJwtService, JwtService>(provider =>
            {
                return new JwtService(provider.GetRequiredService<IOptionsSnapshot<JwtOptions>>().Value);
            });

            services.AddHttpContextAccessor();

            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
