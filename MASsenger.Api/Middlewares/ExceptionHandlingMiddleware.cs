using MASsenger.Application.Responses;
using MASsenger.Core.Enums;
using Serilog;

namespace MASsenger.Api.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                Log.Error("Exception occurred: {Exception}", exception);

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(
                    Result.Failure(ErrorType.Exception, new[] { "Server Error" }));
            }
        }
    }
}
