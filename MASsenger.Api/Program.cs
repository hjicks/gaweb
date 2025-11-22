using MASsenger.Api;
using MASsenger.Api.Middlewares;
using MASsenger.Application;
using MASsenger.Core;
using MASsenger.Infrastracture;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreDI(builder.Configuration)
    .AddApplicationDI()
    .AddInfratructureDI()
    .AddApiDI(builder.Configuration);

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
