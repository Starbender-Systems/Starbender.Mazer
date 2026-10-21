# Blazor Server

Reference `Starbender.Mazer.Server` and `Starbender.Mazer.Mvc` when the app also renders ABP account/MVC pages. Replace the outgoing Server and MVC theme module dependencies in the application. The Server module registers its toolbar contributor and `Blazor.Global` styles/scripts; it depends on `MazerModule`, which supplies MudBlazor services.

<!-- recipe-check: ServerIntegration.cs -->
```csharp
using Starbender.Mazer.Extensions;
using Starbender.Mazer.Mvc;
using Starbender.Mazer.Server;
using Volo.Abp.Modularity;

namespace Mazer.ConsumerExamples;

[DependsOn(typeof(MazerServerModule), typeof(MazerMvcModule))]
public class MazerServerIntegrationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
    }
}
```

Add the adapter to the existing application `[DependsOn]`. Select the themes and route layout as described in [Blazor layout](blazor-layout.md) and [MVC](mvc.md) when migrating from an explicit prior default. Extend `BlazorMazerBundles` for interactive pages and `MazerBundles` for MVC/account pages; see [assets](assets.md).

For a .NET 10 Razor Components host like the Server demo, keep its existing `AddRazorComponents().AddInteractiveServerComponents()` setup, endpoint mapping, authentication and ABP initialization. The following host document illustrates the asset and render-mode placement; its `Routes` is the application router:

<!-- recipe-check: AppServer.razor -->
```razor
@namespace Mazer.ConsumerExamples
@using Microsoft.AspNetCore.Components.Web
@using Starbender.Mazer.Server.Bundling
@using Volo.Abp.AspNetCore.Components.Web.Theming.Bundling
@using static Microsoft.AspNetCore.Components.Web.RenderMode

<!DOCTYPE html>
<html>
<head>
    <base href="/" />
    <AbpStyles BundleName="@BlazorMazerBundles.Styles.Global" />
    <HeadOutlet @rendermode="InteractiveServer" />
</head>
<body>
    <Routes @rendermode="InteractiveServer" />
    <AbpScripts BundleName="@BlazorMazerBundles.Scripts.Global" />
    <script src="_framework/blazor.web.js"></script>
</body>
</html>
```

Merge these elements into the application's existing document, retaining isolated CSS, culture/RTL markup, base path, error UI and head contributions. A classic `_Host.cshtml` application has different bootstrapping; use its existing render setup rather than replacing it with this entire document. The inspected Server demo preconfigures `IsBlazorWebApp=true`; read [authentication](authentication.md) before carrying that flag into another host.

Validate full page load and interactive navigation, circuit reconnect, account-page transitions, light/dark mode, menus/toolbars and feedback listeners. Do not infer that successful compilation validates a live circuit or identity-provider configuration.
