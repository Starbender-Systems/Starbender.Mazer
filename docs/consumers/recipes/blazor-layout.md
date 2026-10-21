# Shared Blazor layout and router

`Starbender.Mazer.Blazor` supplies `MazerModule`, `MainLayout`, Blazor `StarbenderTheme`, `NullLayout`, and `RedirectToLogin`. Use the [Server](blazor-server.md), [WebAssembly](blazor-webassembly.md), or [WebApp](blazor-webapp.md) module for the actual hosting model; those modules depend on `MazerModule`.

`MainLayout` already contains `MudThemeProvider`, `MudPopoverProvider`, `MudDialogProvider`, `MudSnackbarProvider`, the application bar/drawer, branding, ABP menu/toolbar rendering, page alerts, layout hooks, and Mazer's message/notification/progress listeners. Reusing it avoids duplicate providers and listeners. `MazerModule` calls `AddMudServices`; the default snackbar position is bottom-left with a 5-second visible duration. Preserve the application's ABP initialization so conventional dependency registration is active.

Set the router to use Mazer's layout and retain ABP additional assemblies, which include WebAssembly authentication pages. For example, add this routing adapter to the existing application module:

<!-- recipe-check: RoutingIntegration.cs -->
```csharp
using Starbender.Mazer;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.Modularity;
using BlazorThemingOptions = Volo.Abp.AspNetCore.Components.Web.Theming.Theming.AbpThemingOptions;

namespace Mazer.ConsumerExamples;

[DependsOn(typeof(MazerModule))]
public class MazerRoutingIntegrationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
            options.AppAssembly = typeof(MazerRoutingIntegrationModule).Assembly);
        Configure<BlazorThemingOptions>(options =>
            options.DefaultThemeName = Starbender.Mazer.StarbenderTheme.Name);
    }
}
```

Place the adapter in the assembly that owns the application pages, or set `AppAssembly` to the application's actual assembly. The example `Routes.razor` uses that value:

<!-- recipe-check: Routes.razor -->
```razor
@namespace Mazer.ConsumerExamples
@using Microsoft.AspNetCore.Components.Authorization
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.Extensions.Options
@using Starbender.Mazer
@using Volo.Abp.AspNetCore.Components.Web.Theming.Routing
@inject IOptions<AbpRouterOptions> RouterOptions

<Router AppAssembly="@RouterOptions.Value.AppAssembly"
        AdditionalAssemblies="@RouterOptions.Value.AdditionalAssemblies">
    <Found Context="routeData">
        <AuthorizeRouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)">
            <NotAuthorized>
                <RedirectToLogin />
            </NotAuthorized>
        </AuthorizeRouteView>
    </Found>
</Router>
```

This follows the demos' router/layout pattern; retain any application-specific error and not-found behavior. `Starbender.Mazer.StarbenderTheme.Name` is **`StarbenderTheme`**, not MVC's `Starbender`. Its Application, Account and Empty names map to `MainLayout`; an unknown layout returns `MainLayout` when fallback is enabled and `NullLayout` otherwise.

Use application page directives or ABP layout hooks for deliberate variations. Preserve authentication state and endpoint authorization; hiding a route or redirecting the browser does not protect an API. Validate a protected route, an anonymous route and an additional-assembly route; confirm only one provider/listener set is active and the [assets](assets.md) load for that render mode.
