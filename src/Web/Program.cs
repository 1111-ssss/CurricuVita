using Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServiceConfiguration();

var app = builder.Build();

app.UseMiddlewareConfiguration();

app.MapRouteConfiguration();

app.Run();
