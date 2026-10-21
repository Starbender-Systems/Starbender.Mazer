# WebAssembly client and host

The client references `Starbender.Mazer.WebAssembly` and depends on `MazerWebAssemblyModule`. The host references `Starbender.Mazer.Mvc` and `Starbender.Mazer.WebAssembly.Bundling` for MVC/account pages and generated WebAssembly global assets. A direct host reference is appropriate when the bundling assembly is not otherwise available. The inspected host also depends on the Blazor `MazerModule`.

<!-- recipe-check: WebAssemblyIntegration.cs -->
```csharp
using Starbender.Mazer;
using Starbender.Mazer.Extensions;
using Starbender.Mazer.Mvc;
using Starbender.Mazer.WebAssembly;
using Starbender.Mazer.WebAssembly.Bundling;
using Volo.Abp.Modularity;

namespace Mazer.ConsumerExamples;

[DependsOn(typeof(MazerWebAssemblyModule))]
public class MazerWasmClientIntegrationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
    }
}

[DependsOn(typeof(MazerMvcModule), typeof(MazerModule), typeof(MazerWebAssemblyBundlingModule))]
public class MazerWasmHostIntegrationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
    }
}
```

These two adapters belong in different application projects. Replace the old client theme, MVC theme and WebAssembly Basic Theme bundling module as applicable, while preserving other ABP dependencies. Configure [MainLayout and ABP additional assemblies](blazor-layout.md) in the client. Keep existing OIDC or WebApp authentication, HTTP proxy/client and endpoint setup; Mazer does not supply an identity provider.

Extend `BlazorMazerWebAssemblyBundles` on the host via contributors. Its style/script contributors include MudBlazor, Mazer CSS, theme-mode JS and MVC layout assets. The host's generated document loads the resulting files with ABP markers:

```html
<!--ABP:Styles-->
<link href="global.css" rel="stylesheet" />
<!--/ABP:Styles-->
<!--ABP:Scripts-->
<script src="global.js"></script>
<!--/ABP:Scripts-->
```

Those generated assets are a bundling/build responsibility, not files to create by concatenating vendor code. Retain the correct Blazor boot script for the actual host: the inspected WebAssembly demo uses Razor Components with `blazor.web.js` and `InteractiveWebAssemblyRenderMode(prerender: false)`. A standalone WebAssembly app can instead have an existing `blazor.webassembly.js` boot path. Do not swap boot scripts based only on the package name.

The demo also sets `IsBlazorWebApp=true`; this affects login/logout URL selection and is not a default to apply to every standalone WASM/OIDC app. See [authentication](authentication.md) and the [WebApp recipe](blazor-webapp.md) for that distinction.

Validate initial download, client routing, generated asset requests, direct protected URLs, login/logout callbacks, account-page transitions and return URLs. Confirm client and host use compatible package versions and theme values. Production/AOT/trimming behavior must be checked in the actual consuming application.
