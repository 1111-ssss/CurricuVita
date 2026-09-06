using Web.Endpoints;

namespace Web.Extensions;

public static class RouteConfigurationExtensions
{
    public static WebApplication MapRouteConfiguration(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapExternalLoginEndpoints();

        return app;
    }
}