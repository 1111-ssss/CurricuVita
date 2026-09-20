namespace Web.Endpoints;

public static class AuthRedirectHelper
{
    public const string LoginPagePath = "/account/login";
    public const string HomePagePath = "/";

    public static bool IsLocalUrl(string? url) =>
        !string.IsNullOrWhiteSpace(url)
        && url.StartsWith('/')
        && !url.StartsWith("//")
        && !url.StartsWith("/\\");

    public static string ToLocalUrl(string? returnUrl) =>
        IsLocalUrl(returnUrl) ? returnUrl! : HomePagePath;

    public static string BuildRedirect(string pagePath, string errorCode, string? returnUrl)
    {
        var location = $"{pagePath}?error={Uri.EscapeDataString(errorCode)}";

        if (IsLocalUrl(returnUrl))
        {
            location += $"&returnUrl={Uri.EscapeDataString(returnUrl!)}";
        }

        return location;
    }

    public static string BuildLoginRedirect(string errorCode, string? returnUrl) =>
        BuildRedirect(LoginPagePath, errorCode, returnUrl);
}
