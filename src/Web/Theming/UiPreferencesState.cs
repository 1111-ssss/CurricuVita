using Microsoft.JSInterop;

namespace Web.Theming;

public sealed class UiPreferencesState
{
    public UiPreferencesState(IHttpContextAccessor httpContextAccessor)
    {
        var http = httpContextAccessor.HttpContext;
        ThemeMode = UiPreferences.ReadThemeMode(http);
        Accent = UiPreferences.ReadAccent(http);
    }

    public string ThemeMode { get; private set; }
    public string Accent { get; private set; }

    public async Task SetThemeModeAsync(string mode, IJSRuntime js)
    {
        ThemeMode = UiPreferences.NormalizeThemeMode(mode);
        await js.InvokeVoidAsync("uiPreferences.setThemeMode", ThemeMode);
    }

    public async Task SetAccentAsync(string accent, IJSRuntime js)
    {
        Accent = UiPreferences.NormalizeAccent(accent);
        await js.InvokeVoidAsync("uiPreferences.setAccent", Accent);
    }
}
