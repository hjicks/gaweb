using MAS.Application.Results;
using MAS.Core.Enums;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MAS.Api.Middlewares;

public class ExceptionHandlingMiddleware : IMiddleware
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            Log.Error("Exception occurred: {Exception}", exception);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var response = Result.Failure(ErrorType.Exception);
            await context.Response.WriteAsJsonAsync(response, _jsonSerializerOptions);
        }
    }
}
