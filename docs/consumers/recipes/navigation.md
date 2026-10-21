# Add menus, toolbars, and languages

Mazer consumes ABP navigation and toolbar contributors. Keep route ownership, localization resources, and permission definitions in the application. For example, register a menu contributor in the application's UI module:

<!-- recipe-check: NavigationIntegration.cs -->
```csharp
using MudBlazor;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;

namespace Mazer.ConsumerExamples;

public class ApplicationNavigationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
            options.MenuContributors.Add(new ReportsMenuContributor()));
    }
}

public sealed class ReportsMenuContributor : IMenuContributor
{
    public Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            context.Menu.AddItem(new ApplicationMenuItem(
                "Application.Reports", "Reports", url: "/reports",
                icon: Icons.Material.Filled.Assessment,
                requiredPermissionName: "Application.Reports"));
        }

        return Task.CompletedTask;
    }
}
```

Replace the sample route and permission with real application definitions and localize the display text using the application's resource. Permission-filtered navigation is presentation; the destination and server operations must enforce authorization too. The sample icon is for Blazor: Mazer passes the icon string directly to MudBlazor. It does not translate Font Awesome class names into MudBlazor SVG paths. MVC uses its own markup and icon conventions; choose an icon for the actual host.

`Starbender.Mazer.Components.Theme.NavMenu` loads `IMenuManager.GetMainMenuAsync()` and refreshes on ABP application-configuration changes. The first-level renderer handles links, disabled items, nested groups, CSS/element IDs, and ABP component overrides whose component type derives from `ComponentBase`. Prefer the existing contributor/component extension points over copying the drawer.

Use the host's ABP toolbar options to add application items. Blazor and MVC toolbar APIs have similar names in different namespaces; copy the namespace from the consuming host's existing contributor. Server's `Starbender.Mazer.Server.MazerToolbarContributor` adds the mode toggle, login display, and language switch. WebAssembly's counterpart adds login display only when an `AuthenticationStateProvider` exists. MVC's `MazerMainTopToolbarContributor` applies to the main toolbar for the MVC Mazer theme, shows the language switch for multiple languages, and the user menu for authenticated users. Applications can add their own anonymous login link, as the MVC demo does.

Configure supported languages and localization resources through the application's existing ABP options. Keep culture/RTL assets in its host document. Acceptance: permitted and denied users see the appropriate navigation, nested/disabled items behave correctly, custom toolbar items render once, and changing language updates labels and layout direction. Check each hosting model affected by shared menu changes.
