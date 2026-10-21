# Configure palettes and light/dark mode

Use `AddMudTheme` from `Starbender.Mazer.Extensions` in the project that reads theme configuration. It belongs to Shared. `MazerSharedModule` also registers the embedded file set and reads the `MudTheme` section; hosting modules do not all depend on that module. The [hosting recipes](README.md) explicitly call the extension, matching the demos. Choose one registration path per project and preserve existing configuration providers.

For hierarchical configuration, put this object in the application's existing settings. Supply configuration to both server and client when both render the UI; browser-delivered settings must contain public values only.

<!-- recipe-check: theme-settings.json -->
```json
{
  "MudTheme": {
    "PaletteLight": { "Primary": "#435ebe", "Background": "#f2f7ff" },
    "PaletteDark": { "Primary": "#8093e8", "Background": "#151521" },
    "LayoutProperties": { "DefaultBorderRadius": "6px" }
  }
}
```

<!-- recipe-check: ThemeConfiguration.cs -->
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using Starbender.Mazer.Extensions;
using Starbender.Mazer.Utilities;

namespace Mazer.ConsumerExamples;

public static class ThemeConfiguration
{
    public static IServiceCollection AddBrandTheme(
        IServiceCollection services, IConfiguration configuration)
        => services.AddMudTheme(configuration.GetSection("MudTheme"));

    public static string Export(MudTheme theme)
        => MudThemeJsonSerializer.Serialize(theme, indented: true);

    public static MudTheme Import(string json)
        => MudThemeJsonSerializer.Deserialize(json);
}
```

The extension also accepts a section whose scalar value is a JSON string. A missing section falls back to `{}` and default `MudTheme` values; it does not fail merely because the section is absent. Supply settings to get the example palette. Malformed JSON or an explicit JSON `null` can fail startup; validate edited themes before persisting them. Use `MudThemeJsonSerializer` for saved themes: it handles MudColor hex strings or RGBA objects, palette/typography discriminators, and indented output. Keep persistence, tenant selection, authorization, and any editor UI in the consuming application. For replacing an existing theme, start with the [canonical installation guide](../installation.md).

Blazor's scoped `Starbender.Mazer.Services.MudBlazorThemeManager` exposes `Theme`, `IsDarkMode`, `ThemeChanged`, `SetTheme`, `SetDarkMode`, and `ToggleDarkMode`. It observes `IOptionsMonitor<MudTheme>` and disposes its subscription. `AddMudTheme` captures configuration at registration time and does not install configuration-reload tokens. `SetTheme` is an in-memory change, not a persistence API. Use the existing toolbar toggle or call the manager from an interactive component. Custom event subscriptions must marshal rendering through `InvokeAsync` and unsubscribe on disposal.

`MainLayout` reads the preferred mode after the first interactive render. `abpMudBlazorTheme` stores `{ "isDarkMode": true/false }` under `abpMudBlazorTheme.state` in localStorage and updates `data-bs-theme` on the document and body. It falls back to the initial system preference when no stored choice exists; it does not continuously monitor system changes. JS interop is unavailable during prerender, so verify the transition after interactivity starts.

MVC's layout emits palette variables using `Starbender.Mazer.Mvc.Themes.Mazer.Styling.MudThemeCssVariablesBuilder.Build(MudTheme)`: light/dark selectors contain `--abp-mud-*` and Bootstrap `--bs-*` values. A custom MVC layout must preserve the theme palette partial and mode assets to keep those colors synchronized.

Acceptance: verify configured primary/background colors in each active host, toggle both directions, reload to confirm the saved mode, and test first-load/prerender behavior. Recheck custom layouts and both WebApp execution paths. The recipe check exercises configuration and serializer round trips; browser mode persistence remains a runtime check.
