using Web.Endpoints;
using Web.Localization;

namespace Web.Extensions;

public static class RouteConfigurationExtensions
{
    public static WebApplication MapRouteConfiguration(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapExternalLoginEndpoints();
        app.MapCultureEndpoints();
        app.MapDiagEndpoints(); // TEMPORARY, remove after diagnosis

        return app;
    }
}