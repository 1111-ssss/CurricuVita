using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);

builder.Services.AddServiceConfiguration();

var app = builder.Build();

app.UseMiddlewareConfiguration();

await app.AddApplicationConfiguration();

app.MapRouteConfiguration();

app.Run();
