using FluentValidation;
using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Application.Pipelines;
using MAS.Application.Results;
using MAS.Application.Services;
using MAS.Core.Enums;
using MAS.Core.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MAS.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddApplicationDI(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ApplicationDI).Assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationDI).Assembly);

        services.AddSingleton<IConfigureOptions<JwtBearerOptions>>(provider =>
        {
            return new ConfigureNamedOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var tokenOptions = provider.GetRequiredService<IOptions<TokenOptions>>().Value;
                if (string.IsNullOrWhiteSpace(tokenOptions.AccessToken.Key))
                    throw new InvalidOperationException("Jwt key is not configured.");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.AccessToken.Key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";
                        var response = Result.Failure(ErrorType.Unauthorized);
                        await context.Response.WriteAsJsonAsync(response,
                            new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() } });
                    },
                };
            });
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddScoped<IJwtService, JwtService>();

        services.AddScoped<IHashService, HashService>();

        services.AddScoped<IBlobService, BlobService>();

        services.AddSignalR();

        services.AddSingleton<ChatHub>();

        services.AddMemoryCache();
        
        return services;
    }
}
