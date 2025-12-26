using MAS.Api.Middlewares;
using MAS.Application.Results;
using MAS.Core.Constants;
using MAS.Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json.Serialization;

namespace MAS.Api;

public static class ApiDI
{
    public static IServiceCollection AddApiDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new List<string>() { ResponseMessages.Error[ErrorType.InvalidRequestModel] };
                    errors.AddRange(context.ModelState
                        .Where(x => x.Value!.Errors.Any())
                        .SelectMany(x => x.Value!.Errors.Select(e => e.ErrorMessage))
                        .ToList());
                    
                    return new BadRequestObjectResult(Result.Failure(ErrorType.InvalidRequestModel, errors));
                };
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard authorization header using the bearer scheme (\"bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();

            options.EnableAnnotations();
        });

        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

        services.AddTransient<ExceptionHandlingMiddleware>();

        return services;
    }
}
