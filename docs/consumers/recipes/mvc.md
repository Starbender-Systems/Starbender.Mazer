# MVC and Razor Pages

Use `Starbender.Mazer.Mvc` for ABP MVC/Razor Pages, including account endpoints alongside Blazor. Its `MazerMvcModule` registers the MVC theme, embedded views, toolbar contributor, and MVC bundles. Select a package version compatible with the application's actual ABP baseline; see [adoption](../adoption.md).

Remove the replaced Basic Theme dependency from the owning web module, preserve the rest of its dependencies, and add this adapter to its `[DependsOn]` (or merge these settings into that module):

<!-- recipe-check: MvcIntegration.cs -->
```csharp
using Starbender.Mazer.Extensions;
using Starbender.Mazer.Mvc;
using Starbender.Mazer.Mvc.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.Modularity;
using MvcThemingOptions = Volo.Abp.AspNetCore.Mvc.UI.Theming.AbpThemingOptions;

namespace Mazer.ConsumerExamples;

[DependsOn(typeof(MazerMvcModule))]
public class MazerMvcIntegrationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
        Configure<MvcThemingOptions>(options =>
            options.DefaultThemeName = Starbender.Mazer.Mvc.StarbenderTheme.Name);
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(MazerBundles.Styles.Global,
                bundle => bundle.AddFiles("/global-styles.css"));
            options.ScriptBundles.Configure(MazerBundles.Scripts.Global,
                bundle => bundle.AddFiles("/global-scripts.js"));
        });
    }
}
```

The sample filenames belong to the application; create them or replace/remove those contributions. MVC's theme name is **`Starbender`**, obtained from `Starbender.Mazer.Mvc.StarbenderTheme.Name`; it differs from the Blazor theme name. Explicit selection is useful when an existing theme already configured the default.

The theme maps ABP Application, Account, and Empty layouts to `~/Themes/Mazer/Layouts/Application.cshtml`, `Account.cshtml`, and `Empty.cshtml`. It adds the Mazer MVC global bundles on top of ABP standard bundles and emits light/dark palette variables from `IOptionsMonitor<MudTheme>`. Reuse these layouts and [theme configuration](theming.md) rather than copying views into the application for color changes. Keep existing ABP client-library installation and static asset middleware; Mazer is not a replacement for those prerequisites.

Application authorization, login endpoints, localization and menu/toolbar registration remain in the web module. See [navigation](navigation.md), [authentication](authentication.md), and [assets](assets.md).

Validate an application page and the login/account pages, both anonymous and authenticated. Check bundle requests, branding, responsive menu behavior, language selection and both palette modes. Compare with the [pinned MVC demo](../source-evidence.md) when upgrading an existing application.
