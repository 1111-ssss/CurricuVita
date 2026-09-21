using Microsoft.AspNetCore.Localization;

namespace Web.Localization;

public static class CultureEndpoints
{
    private static readonly HashSet<string> SupportedCultures = new(StringComparer.OrdinalIgnoreCase)
    {
        "en",
        "ru"
    };

    public static IEndpointRouteBuilder MapCultureEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/set-culture", SetCulture);
        return endpoints;
    }

    private static IResult SetCulture(string culture, string? redirectUri, HttpContext http)
    {
        if (!SupportedCultures.Contains(culture))
        {
            culture = "en";
        }

        http.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                Path = "/",
                SameSite = SameSiteMode.Lax
            });

        return Results.LocalRedirect(GetSafeRedirect(redirectUri));
    }

    private static string GetSafeRedirect(string? redirectUri)
    {
        if (string.IsNullOrWhiteSpace(redirectUri))
        {
            return "/";
        }

        if (redirectUri.StartsWith('/') && !redirectUri.StartsWith("//") && !redirectUri.StartsWith("/\\"))
        {
            return redirectUri;
        }

        return "/";
    }
}
