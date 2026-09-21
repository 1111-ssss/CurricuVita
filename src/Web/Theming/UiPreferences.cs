namespace Web.Theming;

public static class UiPreferences
{
    public const string ThemeModeCookie = "cv-theme-mode";
    public const string AccentCookie = "cv-accent";
    public const string DefaultThemeMode = "light";
    public const string DefaultAccent = "blue";
    public const int CookieMaxAgeSeconds = 60 * 60 * 24 * 365;

    public static readonly string[] ThemeModes = ["light", "dark", "auto"];
    public static readonly string[] Accents = ["blue", "indigo", "teal", "rose", "amber"];

    public static string NormalizeThemeMode(string? value) =>
        ThemeModes.Contains(value, StringComparer.OrdinalIgnoreCase)
            ? value!.ToLowerInvariant()
            : DefaultThemeMode;

    public static string NormalizeAccent(string? value) =>
        Accents.Contains(value, StringComparer.OrdinalIgnoreCase)
            ? value!.ToLowerInvariant()
            : DefaultAccent;

    public static string ReadThemeMode(HttpContext? http) =>
        NormalizeThemeMode(http?.Request.Cookies[ThemeModeCookie]);

    public static string ReadAccent(HttpContext? http) =>
        NormalizeAccent(http?.Request.Cookies[AccentCookie]);

    public static string ResolveColorMode(string themeMode) =>
        themeMode == "dark" ? "dark" : "light";
}
