using MAS.Api;
using MAS.Api.Middlewares;
using MAS.Application;
using MAS.Application.Hubs;
using MAS.Application.Interfaces;
using MAS.Core;
using MAS.Infrastracture;
using MAS.Infrastracture.Database;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

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
    app.UseSwaggerUI(c =>
    {
        c.DocExpansion(DocExpansion.None); // Shut up swagger
    });

    using var seedScope = app.Services.CreateScope();
    var dbContext = seedScope.ServiceProvider.GetRequiredService<EfDbContext>();
    var hashService = seedScope.ServiceProvider.GetRequiredService<IHashService>();
    await dbContext.Database.EnsureCreatedAsync();
    await DevelopSeed.Seed(dbContext, hashService);
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("MASCorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.MapHub<ChatHub>("/hub");

app.Run();
