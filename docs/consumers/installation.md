# Install Mazer and replace an ABP theme

This is the canonical installation and theme-replacement guide for existing ABP **MVC/Razor Pages, Blazor Server, WebAssembly, and Blazor WebApp** applications. It covers migration from **Basic Theme, LeptonX, and LeptonX Lite**. Use this sequence with the linked hosting recipes; the recipes describe host-specific code and assets.

The inspected source declares Mazer **1.0.4**, **net10.0**, ABP **10.0.2**, and MudBlazor **8.6.0**. Select a published Mazer version compatible with your application's resolved dependencies. Source version declarations do not establish feed availability or compatibility with other ABP/MudBlazor versions. See [version evidence](adoption.md#establish-version-evidence) and the [package map](packages.md).

## 1. Inventory the existing integration

Save a recoverable commit of the current application. Run its established restore/build first, then record the UI model, old theme, package versions, root modules, startup paths, and local customizations. For each UI project:

```text
dotnet list path/to/Application.csproj package --include-transitive
```

Search the application, including central version files and frontend manifests:

```text
rg -n "BasicTheme|Theme.Basic|LeptonX|LeptonXLite|lepton|DefaultThemeName|MainLayout|_Layout|ThemeBundles" .
```

Inspect project references and any downloaded theme source too. An existing project may have dependencies on both MVC and Blazor themes. Include a separate AuthServer or another MVC web host when its account pages must adopt Mazer; changing the Blazor client does not change another process's account UI. Identify any old theme-management module before removing it: migrate its application-owned settings/features deliberately rather than removing identity, tenant, or business modules along with the theme.

## 2. Add packages to the correct projects

Choose only the row for your hosting model. All project paths below are examples to replace with your application's paths.

| UI model | Project | Direct Mazer package references |
| --- | --- | --- |
| MVC/Razor Pages | Web, including a separate MVC AuthServer if applicable | `Starbender.Mazer.Mvc` |
| Blazor Server with MVC/account pages | Server web project | `Starbender.Mazer.Server`, `Starbender.Mazer.Mvc` |
| Hosted WebAssembly | Client | `Starbender.Mazer.WebAssembly` |
| Hosted WebAssembly | Host with MVC/account pages and client bundling | `Starbender.Mazer.Mvc`, `Starbender.Mazer.WebAssembly.Bundling`; also `Starbender.Mazer.Blazor` when directly using `MazerModule` as in the source demo |
| Blazor WebApp/InteractiveAuto | Server | `Starbender.Mazer.Server`, `Starbender.Mazer.Mvc`, `Starbender.Mazer.WebAssembly.Bundling` |
| Blazor WebApp/InteractiveAuto | Client | `Starbender.Mazer.WebAssembly` |

For a standalone WASM client, use the client row; configure any independent server/account UI in its own project. The installer package is not needed for manual runtime integration. Shared and core Blazor dependencies arrive through the UI packages; explicitly reference them if your application's package conventions require direct references for directly used APIs.

For projects **without Central Package Management**, the following PowerShell example adds the Server package set without restoring halfway through the migration. Replace the version with one confirmed in your configured feed; repeat the same form for the packages/projects in your selected row.

```powershell
$MazerVersion = '1.0.4' # Example source baseline; confirm availability/compatibility first.
dotnet add src/MyApp.Blazor/MyApp.Blazor.csproj package Starbender.Mazer.Server --version $MazerVersion --no-restore
dotnet add src/MyApp.Blazor/MyApp.Blazor.csproj package Starbender.Mazer.Mvc --version $MazerVersion --no-restore
```

For **Central Package Management**, edit the existing applicable `Directory.Packages.props` and project `ItemGroup` instead. For example, the Server project uses these entries (merge into existing groups; do not replace the files):

```xml
<!-- Directory.Packages.props: use the selected published version. -->
<ItemGroup>
  <PackageVersion Include="Starbender.Mazer.Server" Version="1.0.4" />
  <PackageVersion Include="Starbender.Mazer.Mvc" Version="1.0.4" />
</ItemGroup>
```

```xml
<!-- MyApp.Blazor.csproj: versions remain in the central file. -->
<ItemGroup>
  <PackageReference Include="Starbender.Mazer.Server" />
  <PackageReference Include="Starbender.Mazer.Mvc" />
</ItemGroup>
```

Keep Mazer versions aligned across rendering projects. Preserve existing feed authentication and package-source mapping. If a chosen package cannot restore, resolve its availability/version first; do not compensate by upgrading unrelated ABP dependencies.

## 3. Replace outgoing packages and module dependencies

Remove the outgoing theme's **direct** package references from each affected project and its `[DependsOn]` entries from the owning ABP module. Use the table matching the installed theme; entries are package IDs and module type names, not interchangeable identifiers.

| Theme / UI surface | Outgoing package | Outgoing module |
| --- | --- | --- |
| Basic / MVC | `Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic` | `AbpAspNetCoreMvcUiBasicThemeModule` |
| Basic / Server | `Volo.Abp.AspNetCore.Components.Server.BasicTheme` | `AbpAspNetCoreComponentsServerBasicThemeModule` |
| Basic / WASM client | `Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme` | `AbpAspNetCoreComponentsWebAssemblyBasicThemeModule` |
| Basic / WASM bundling host | `Volo.Abp.AspNetCore.Components.WebAssembly.BasicTheme.Bundling` | `AbpAspNetCoreComponentsWebAssemblyBasicThemeBundlingModule` |
| LeptonX / MVC | `Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX` | `AbpAspNetCoreMvcUiLeptonXThemeModule` |
| LeptonX / Server | `Volo.Abp.AspNetCore.Components.Server.LeptonXTheme` | `AbpAspNetCoreComponentsServerLeptonXThemeModule` |
| LeptonX / WASM client | `Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme` | `AbpAspNetCoreComponentsWebAssemblyLeptonXThemeModule` |
| LeptonX / WASM bundling host | `Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXTheme.Bundling` | `AbpAspNetCoreComponentsWebAssemblyLeptonXThemeBundlingModule` |
| LeptonX Lite / MVC | `Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite` | `AbpAspNetCoreMvcUiLeptonXLiteThemeModule` |
| LeptonX Lite / Server | `Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme` | `AbpAspNetCoreComponentsServerLeptonXLiteThemeModule` |
| LeptonX Lite / WASM client | `Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme` | `AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeModule` |
| LeptonX Lite / WASM bundling host | `Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme.Bundling` | `AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeBundlingModule` |

For example, a Server app replacing LeptonX Lite removes these references in addition to updating its module:

```text
dotnet remove src/MyApp.Blazor/MyApp.Blazor.csproj package Volo.Abp.AspNetCore.Components.Server.LeptonXLiteTheme
dotnet remove src/MyApp.Blazor/MyApp.Blazor.csproj package Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite
```

Use the corresponding IDs for Basic Theme or LeptonX. In a centrally managed solution, remove old `PackageVersion` entries only after no remaining project uses them. Remove direct references to old core/theme-source projects only when no longer required. A transitive theme reference requires investigating its owning module; removing a direct reference alone cannot remove it.

Replace each removed module dependency with its Mazer counterpart:

| Removed surface | Mazer dependency | Recipe |
| --- | --- | --- |
| MVC theme | `Starbender.Mazer.Mvc.MazerMvcModule` | [MVC](recipes/mvc.md) |
| Server theme | `Starbender.Mazer.Server.MazerServerModule` | [Server](recipes/blazor-server.md) |
| WASM client theme | `Starbender.Mazer.WebAssembly.MazerWebAssemblyModule` | [WebAssembly](recipes/blazor-webassembly.md) |
| WASM bundling theme | `Starbender.Mazer.WebAssembly.Bundling.MazerWebAssemblyBundlingModule` | [WebAssembly](recipes/blazor-webassembly.md) |

WebApp uses **all three server surfaces** (MVC, Server, WASM bundling) and the WASM client surface. Follow the [WebApp recipe](recipes/blazor-webapp.md) in both projects, including `IsBlazorWebApp`, `Parameters.InteractiveAuto`, and the existing client authentication services. A classic Server host and a standalone WASM/OIDC client retain their own startup/authentication model.

The recipe adapters are examples: add the adapter to the existing root module's `[DependsOn]`, or merge its dependencies/settings into that root module. A NuGet reference alone does not activate a module. Preserve ABP shared infrastructure, application modules, and Blazorise providers still required by other module pages.

## 4. Select Mazer explicitly

Mazer registers its themes but only fills an unset default name. Replace any old `DefaultThemeName` assignment, including a later `PostConfigure` callback. MVC uses **`Starbender`**; Blazor uses **`StarbenderTheme`**. Use the appropriate constant rather than a copied string.

These optional adapters explicitly select Mazer after ordinary options configuration. Put only the adapter(s) for the project's UI surface in that project and add them to the root module dependencies, or merge their `context.Services.PostConfigure` calls into its existing `ConfigureServices`. MVC-only applications do not need the Blazor adapter. For WebApp, select the Blazor theme in both rendering projects and the MVC theme in the server/account host.

<!-- recipe-check: MvcThemeSelection.cs -->
```csharp
using Microsoft.Extensions.DependencyInjection;
using Starbender.Mazer.Mvc;
using Volo.Abp.Modularity;
using MvcThemingOptions = Volo.Abp.AspNetCore.Mvc.UI.Theming.AbpThemingOptions;

namespace Mazer.ConsumerExamples;

[DependsOn(typeof(MazerMvcModule))]
public class MazerMvcThemeSelectionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PostConfigure<MvcThemingOptions>(options =>
            options.DefaultThemeName = Starbender.Mazer.Mvc.StarbenderTheme.Name);
    }
}
```

<!-- recipe-check: BlazorThemeSelection.cs -->
```csharp
using Microsoft.Extensions.DependencyInjection;
using Starbender.Mazer;
using Volo.Abp.Modularity;
using BlazorThemingOptions = Volo.Abp.AspNetCore.Components.Web.Theming.Theming.AbpThemingOptions;

namespace Mazer.ConsumerExamples;

[DependsOn(typeof(MazerModule))]
public class MazerBlazorThemeSelectionModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PostConfigure<BlazorThemingOptions>(options =>
            options.DefaultThemeName = Starbender.Mazer.StarbenderTheme.Name);
    }
}
```

Keep the hosting module dependencies from step 3 as well: the selection adapters do not replace Server/WASM hosting integration. Check for conflicting callbacks still selecting the old theme; multiple `PostConfigure` actions run in registration order.

## 5. Move layouts, bundles, and customizations

Use the [Blazor router/layout recipe](recipes/blazor-layout.md) for `MainLayout` and ABP additional assemblies. Update `Routes.razor`, `_Imports.razor`, `App.razor`, `_Host.cshtml`, and any custom layout/component code that names the old theme. Preserve authentication state, existing render modes, base paths, isolated CSS, culture/RTL markup, and error UI. Mazer's layout already supplies MudBlazor providers and feedback listeners.

Retarget old `BasicThemeBundles`, `BlazorBasicThemeBundles`, `LeptonXThemeBundles`, `BlazorLeptonXThemeBundles`, `LeptonXLiteThemeBundles`, or `BlazorLeptonXLiteThemeBundles` references according to the actual host. Search for the installed version's bundle types as names/namespaces can differ.

| Surface | Replacement constants | Global ID |
| --- | --- | --- |
| MVC | `Starbender.Mazer.Mvc.Bundling.MazerBundles` | `Mazer.Global` |
| Server | `Starbender.Mazer.Server.Bundling.BlazorMazerBundles` | `Blazor.Global` |
| WASM bundling | `Starbender.Mazer.WebAssembly.Bundling.BlazorMazerWebAssemblyBundles` | `BlazorWebAssembly.Global` |

Move application CSS/JS contributions to these bundles and update the rendered bundle references. WASM uses generated `global.css` / `global.js`; WebApp passes them through `WebAssemblyStyleFiles` / `WebAssemblyScriptFiles` in addition to server bundles. See [assets](recipes/assets.md) and the hosting recipe for the exact markup. Keep files actually used by the application; remove old theme-only styles/scripts and frontend package dependencies when nothing still uses them. Refresh client libraries using the application's existing ABP tooling, commonly `abp install-libs`, in its documented working directory. Do not delete unrelated Bootstrap, Blazorise, or ABP standard assets or add duplicate MudBlazor/Blazor scripts.

Review copied MVC views (`Themes`, `Pages`, `Views`, `_ViewImports.cshtml`, `_ViewStart.cshtml`), virtual-file overrides, and Blazor components inheriting or replacing old theme types. Port their intended branding/navigation behavior onto Mazer extension points instead of merely renaming an inheritance/import. Preserve application authorization and business logic. Theme-specific commercial UI/management integrations may need their own adapters; Mazer does not promise automatic compatibility for every commercial module or copied theme component.

## 6. Configure palettes and preserve authentication

The [theming recipe](recipes/theming.md) shows `AddMudTheme`, the optional `MudTheme` settings, and serialization. A missing `MudTheme` section falls back to `{}` and default `MudTheme` values; custom colors require real settings. Malformed JSON or an explicit JSON `null` is different and can fail deserialization. `AddMudTheme` captures settings during registration and does not install reload tokens. Configure matching values in every process that renders the theme.

Preserve the existing identity provider, cookies/OIDC, account endpoints, middleware, client services, and return-URL policy. The [authentication recipe](recipes/authentication.md) explains the hosting flag and redirect differences. Mazer provides theme UI around those services. A separate AuthServer needs its own MVC package/module/default selection, palette configuration if customized, and asset validation.

## 7. Restore, build, and verify the migration

After the references, modules, and assets are updated, run the application's established restore/build/tests and review its lockfile changes. For example:

```text
dotnet restore path/to/Application.slnx
dotnet build path/to/Application.slnx --configuration Release --no-restore
dotnet list path/to/Application.csproj package --include-transitive
```

Use the real solution extension/path. Re-run the search from step 1 and classify remaining old-theme references; investigate transitive dependencies instead of treating a successful package removal as a complete migration. Verify the resolved MVC and Blazor default theme names, then exercise:

- Anonymous and authenticated application routes, account/login pages (including a separate AuthServer), login callbacks/return location, and logout.
- Menus, branding, custom components, language/RTL switching, responsive layout, confirmations, notifications, and progress cleanup.
- CSS/JS requests without 404s, correct bundle families, retained isolated CSS, and one applicable Blazor boot script/provider set.
- Light/dark mode and persistence after reload; for InteractiveAuto, both first server execution and subsequent WASM execution.

Record the starting theme, package versions, projects, hosts, and flows actually tested. Mazer's repository checks compile marked examples and check selected configuration behavior against source; they do not execute this package-installation workflow against a published feed or prove a live Basic/LeptonX/Lite migration. If runtime prerequisites prevent a check, report it as unverified. Recover by restoring the application's saved references, modules, settings, frontend manifests/lockfiles, and customizations together.

## Upstream reference points

The outgoing package/module names and theme-switching surfaces were checked against ABP documentation; verify the installed version when adapting a different application baseline. Mazer-specific target names above come from the [inspected source](source-evidence.md).

- [ABP LeptonX Lite Blazor, including Basic Theme replacement](https://abp.io/docs/10.0/ui-themes/lepton-x-lite/blazor)
- [ABP LeptonX Lite Server installation](https://abp.io/docs/10.0/ui-themes/lepton-x-lite/blazor?UI=BlazorServer)
- [ABP LeptonX Blazor installation](https://abp.io/docs/10.0/ui-themes/lepton-x/blazor)
- [ABP LeptonX Server installation](https://abp.io/docs/10.0/ui-themes/lepton-x/blazor?UI=BlazorServer)
- [ABP LeptonX Lite MVC installation](https://abp.io/docs/latest/ui-themes/lepton-x-lite/asp-net-core?LanguageCode=en)
- [ABP LeptonX MVC installation](https://abp.io/docs/latest/ui-themes/lepton-x/mvc)
