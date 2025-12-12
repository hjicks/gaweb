using MASsenger.Api;
using MASsenger.Api.Middlewares;
using MASsenger.Application;
using MASsenger.Application.Hubs;
using MASsenger.Core;
using MASsenger.Infrastracture;
using MASsenger.Infrastracture.Database;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCoreDI(builder.Configuration)
    .AddApplicationDI()
    .AddInfratructureDI()
    .AddApiDI(builder.Configuration);

builder.Host.UseSerilog();
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var seedScope = app.Services.CreateScope();
    var dbContext = seedScope.ServiceProvider.GetRequiredService<EfDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await DevelopSeed.Seed(dbContext);
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();
app.MapHub<ChatHub>("/hub");

app.Run();
