using MASsenger.Application.Responses;
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

                await context.Response.WriteAsJsonAsync(Result.Failure("Server Error"));
            }
        }
    }
}
