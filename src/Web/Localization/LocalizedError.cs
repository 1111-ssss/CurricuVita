using Microsoft.Extensions.Localization;

namespace Web.Localization;

public static class LocalizedError
{
    public static string Message(IStringLocalizer localizer, string? code, string fallback)
    {
        if (string.IsNullOrEmpty(code))
        {
            return fallback;
        }

        var localized = localizer[$"Errors_{code}"];
        return localized.ResourceNotFound ? fallback : localized.Value;
    }
}
