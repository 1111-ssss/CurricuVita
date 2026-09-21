using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddServiceConfiguration(builder.Configuration);
builder.Services.AddIdentityConfiguration(builder.Configuration);

var app = builder.Build();

app.UseMiddlewareConfiguration();
await app.AddApplicationConfiguration();
app.MapRouteConfiguration();

app.Run();
