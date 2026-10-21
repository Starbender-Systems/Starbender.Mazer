# Blazor WebApp and InteractiveAuto

An InteractiveAuto app needs Mazer in **both** rendering processes. The server references Server, Mvc and WebAssembly.Bundling; the client references WebAssembly. Keep the existing server/client project reference, render services, authentication-state flow, endpoint mapping and ABP initialization.

The following additive adapters reproduce the source demo's theme/bundling choices. Put each adapter in its owning project and include it in the respective root module's `[DependsOn]`:

<!-- recipe-check: WebAppIntegration.cs -->
```csharp
using Starbender.Mazer.Extensions;
using Starbender.Mazer.Mvc;
using Starbender.Mazer.Server;
using Starbender.Mazer.WebAssembly;
using Starbender.Mazer.WebAssembly.Bundling;
using Volo.Abp.AspNetCore.Components.Web;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;

namespace Mazer.ConsumerExamples;

[DependsOn(typeof(MazerMvcModule), typeof(MazerServerModule), typeof(MazerWebAssemblyBundlingModule))]
public class MazerWebAppServerIntegrationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options => options.IsBlazorWebApp = true);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
        Configure<AbpBundlingOptions>(options => options.Parameters.InteractiveAuto = true);
    }
}

[DependsOn(typeof(MazerWebAssemblyModule))]
public class MazerWebAppClientIntegrationModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpAspNetCoreComponentsWebOptions>(options => options.IsBlazorWebApp = true);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
    }
}
```

Keep `AddRazorComponents().AddInteractiveServerComponents().AddInteractiveWebAssemblyComponents()` and the matching endpoint render modes from the application's existing startup. The source client uses `AddBlazorWebAppServices()` for its authentication path; retain or adapt the actual application's configured authentication, not a second standalone OIDC setup.

The server serves `BlazorMazerBundles`; client execution needs `global.css` / `global.js` generated from `BlazorMazerWebAssemblyBundles`. Configure application contributors in both pipelines when the same asset is needed in both processes. MVC/account endpoints also use their own `MazerBundles`.

<!-- recipe-check: AppAuto.razor -->
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
    <AbpStyles BundleName="@BlazorMazerBundles.Styles.Global"
               WebAssemblyStyleFiles="GlobalStyles" @rendermode="InteractiveAuto" />
    <HeadOutlet @rendermode="InteractiveAuto" />
</head>
<body>
    <Routes @rendermode="InteractiveAuto" />
    <AbpScripts BundleName="@BlazorMazerBundles.Scripts.Global"
                WebAssemblyScriptFiles="GlobalScripts" @rendermode="InteractiveAuto" />
    <script src="_framework/blazor.web.js"></script>
</body>
</html>

@code {
    private List<string> GlobalStyles => ["global.css"];
    private List<string> GlobalScripts => ["global.js"];
}
```

Merge into the existing document and retain the server **and client** isolated-CSS links, culture/RTL markup and error UI. Use [MainLayout](blazor-layout.md), one set of providers/listeners per active layout, and identical [MudTheme configuration](theming.md) where both server and client render.

Validate a cold first visit, server interactivity, then a return visit/client execution after assets cache. Check full reloads, authentication transitions, both asset pipelines, mode persistence and feedback components. A green server render alone does not validate InteractiveAuto client execution.
