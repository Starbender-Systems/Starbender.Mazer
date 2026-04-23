# Migration Guide

This guide covers migrating an existing ABP Framework application from the Basic Theme or another ABP UI theme to Starbender.Mazer.

## Table of Contents

- [Supported UI Types](#supported-ui-types)
- [General Principles](#general-principles)
- [Package and Module Map](#package-and-module-map)
- [Verification Checklist](#verification-checklist)
- [Demo Reference Map](#demo-reference-map)

## Supported UI Types

| UI Model | | |
| --- | --- | --- |
| MVC / Razor Pages | [Instructions](#mvc--razor-pages) | [Sample Source](../demo/MvcUi/) |
| Blazor Server | [Instructions](#blazor-server) | [Sample Source](../demo/Server/) |
| Blazor WebAssembly | [Instructions](#blazor-webassembly) | [Sample Source](../demo/WebAssembly/) |
| Blazor WebApp | [Instructions](#blazor-webapp) | [Sample Source](../demo/WebApp/) |

See also: [Package and Module Map](#package-and-module-map)

This document is based on the current Starbender.Mazer package layout in this repository and cross-checked against the latest ABP documentation for:

- UI theming and Basic Theme installation
- ASP.NET Core MVC / Razor Pages bundling
- Blazor global scripts and styles

If this guide ever conflicts with the ABP documentation, treat the ABP documentation as authoritative:

- https://abp.io/docs/latest/framework/ui/mvc-razor-pages/bundling-minification
- https://abp.io/docs/latest/framework/ui/blazor/basic-theme
- https://abp.io/docs/latest/framework/ui/blazor/global-scripts-styles

## General Principles

The migration usually has four parts:

1. Replace the ABP theme package and module dependencies with the corresponding Starbender.Mazer packages and modules.
2. Move any application-specific CSS and JavaScript bundle contributions to the correct bundle names for the target UI type.
3. Update the Blazor router/layout entry points so `MainLayout` comes from Starbender.Mazer.
4. Re-test account/login pages, main navigation, toolbars, and any custom pages that previously depended on the old theme.

Before you start:

- Commit or back up your current theme customizations.
- Identify whether your app still contains MVC / Razor Pages endpoints for login, account management, or host pages.
- Search for theme-specific references such as `BasicTheme`, `BlazorBasicThemeBundles`, and `AbpAspNetCoreMvcUiBasicThemeModule`.

## Package and Module Map

Use these Starbender.Mazer packages and modules as the migration targets:

- MVC / Razor Pages:
  - Package: `Starbender.Mazer.Mvc`
  - Module: `MazerMvcModule`
- Blazor Server:
  - Packages: `Starbender.Mazer.Mvc`, `Starbender.Mazer.Server`
  - Modules: `MazerMvcModule`, `MazerServerModule`
- Blazor WebAssembly:
  - Client package: `Starbender.Mazer.WebAssembly`
  - Client module: `MazerWebAssemblyModule`
  - Host package: `Starbender.Mazer.Mvc`
  - Host module: `MazerMvcModule`
  - Host bundling module: `MazerWebAssemblyBundlingModule`
- Blazor WebApp:
  - Server packages: `Starbender.Mazer.Mvc`, `Starbender.Mazer.Server`, `Starbender.Mazer.WebAssembly.Bundling`
  - Server modules: `MazerMvcModule`, `MazerServerModule`, `MazerWebAssemblyBundlingModule`
  - Client package: `Starbender.Mazer.WebAssembly`
  - Client module: `MazerWebAssemblyModule`

Notes:

- `MazerMvcModule` is still important for applications that expose MVC / Razor Pages endpoints, including account and host pages.
- `MazerWebAssemblyBundlingModule` is the piece that adds `Starbender.Mazer.Blazor` assets into ABP's Blazor WebAssembly global asset pipeline.
- In some hosted Blazor WebAssembly solutions, the host may resolve `MazerWebAssemblyBundlingModule` transitively through the client project. If that is not true in your solution, add a direct package reference for the needed assembly.

## MVC / Razor Pages

Use this path for a traditional ABP MVC / Razor Pages UI.

### 1. Add the theme package

Install `Starbender.Mazer.Mvc` in the web project.

### 2. Replace the theme module

In your web module:

- Remove `AbpAspNetCoreMvcUiBasicThemeModule` if it is present.
- Add `MazerMvcModule`.

Representative change:

```csharp
[DependsOn(
    // Remove:
    // typeof(AbpAspNetCoreMvcUiBasicThemeModule),

    // Add:
    typeof(MazerMvcModule)
)]
```

### 3. Configure app-level CSS and JavaScript using ABP bundles

ABP's bundling system is the source of truth here. For MVC pages, attach your application-level CSS and JavaScript to `MazerBundles`.

```csharp
private void ConfigureBundles()
{
    Configure<AbpBundlingOptions>(options =>
    {
        options.StyleBundles.Configure(
            MazerBundles.Styles.Global,
            bundle => { bundle.AddFiles("/global-styles.css"); }
        );

        options.ScriptBundles.Configure(
            MazerBundles.Scripts.Global,
            bundle => { bundle.AddFiles("/global-scripts.js"); }
        );
    });
}
```

If you currently contribute files to Basic Theme bundle names, move those contributions to `MazerBundles.Styles.Global` and `MazerBundles.Scripts.Global`.

### 4. Configure the MudBlazor theme

Load your theme from configuration or override it in code:

```csharp
context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
```

### 5. Clean up old theme references

- Remove Basic Theme-specific usings and package references when no longer needed.
- Re-test account, login, and layout pages because they often surface the last remaining theme dependencies.

## Blazor Server

Use this path for an ABP Blazor Server application that still hosts MVC / account pages on the server.

### 1. Add the theme packages

Install these packages in the server web project:

- `Starbender.Mazer.Mvc`
- `Starbender.Mazer.Server`

### 2. Replace the theme modules

In the main web module:

- Remove `AbpAspNetCoreMvcUiBasicThemeModule` if present.
- Remove `AbpAspNetCoreComponentsServerBasicThemeModule` if present.
- Add `MazerMvcModule`.
- Add `MazerServerModule`.

Representative change:

```csharp
[DependsOn(
    // Remove:
    // typeof(AbpAspNetCoreMvcUiBasicThemeModule),
    // typeof(AbpAspNetCoreComponentsServerBasicThemeModule),

    // Add:
    typeof(MazerMvcModule),
    typeof(MazerServerModule)
)]
```

### 3. Configure Blazor server hosting and router integration

If your app uses the current ABP Blazor Server hosting model, keep the standard ABP router setup and use `MainLayout` from Starbender.Mazer.

In `Routes.razor`:

```razor
@using Starbender.Mazer
@using Volo.Abp.AspNetCore.Components.Web.Theming.Routing
@using Microsoft.Extensions.Options

@inject IOptions<AbpRouterOptions> RouterOptions

<Router AppAssembly="typeof(Program).Assembly" AdditionalAssemblies="RouterOptions.Value.AdditionalAssemblies">
    <Found Context="routeData">
        <AuthorizeRouteView RouteData="routeData" DefaultLayout="typeof(MainLayout)">
            <NotAuthorized>
                <RedirectToLogin />
            </NotAuthorized>
        </AuthorizeRouteView>
    </Found>
</Router>
```

### 4. Configure bundles for both MVC and Blazor

For server apps, there are usually two bundle surfaces:

- `MazerBundles` for MVC / Razor Pages
- `BlazorMazerBundles` for the Blazor app shell

Representative setup:

```csharp
private void ConfigureBundles()
{
    Configure<AbpBundlingOptions>(options =>
    {
        options.StyleBundles.Configure(
            MazerBundles.Styles.Global,
            bundle => { bundle.AddFiles("/global-styles.css"); }
        );

        options.ScriptBundles.Configure(
            MazerBundles.Scripts.Global,
            bundle => { bundle.AddFiles("/global-scripts.js"); }
        );

        options.StyleBundles.Configure(
            BlazorMazerBundles.Styles.Global,
            bundle => { bundle.AddFiles("/global-styles.css"); }
        );

        options.ScriptBundles.Configure(
            BlazorMazerBundles.Scripts.Global,
            bundle => { bundle.AddFiles("/global-scripts.js"); }
        );
    });
}
```

`BlazorMazerBundles` maps to ABP's `BlazorStandardBundles`, but gives consumers a theme-specific surface similar to `BlazorBasicThemeBundles`.

In `App.razor`, reference the Mazer Blazor bundles:

```razor
<AbpStyles BundleName="@BlazorMazerBundles.Styles.Global" />
<AbpScripts BundleName="@BlazorMazerBundles.Scripts.Global" />
```

### 5. Configure MudBlazor and optional Blazorise compatibility

Starbender.Mazer is MudBlazor-based, but many ABP apps still keep Blazorise dependencies for ABP modules or phased migrations.

```csharp
context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
```

If your app still requires Blazorise during migration, keeping the standard Bootstrap5 and FontAwesome provider registrations is reasonable.

## Blazor WebAssembly

Use this path for an ABP hosted Blazor WebAssembly application with a server host project and a WebAssembly client project.

### 1. Add the theme packages

Client project:

- Install `Starbender.Mazer.WebAssembly`

Host project:

- Install `Starbender.Mazer.Mvc`
- Ensure the host can also resolve `MazerWebAssemblyBundlingModule`

In some solutions that happens transitively through the client project reference. If not, add a direct reference to the needed Starbender.Mazer WebAssembly bundling package / assembly.

### 2. Replace the theme modules

In the client module:

- Remove `AbpAspNetCoreComponentsWebAssemblyBasicThemeModule` if present.
- Add `MazerWebAssemblyModule`.

In the host module:

- Remove `AbpAspNetCoreMvcUiBasicThemeModule` if present.
- Remove `AbpAspNetCoreComponentsWebAssemblyBasicThemeBundlingModule` if present.
- Add `MazerMvcModule`.
- Add `MazerWebAssemblyBundlingModule`.

If your host performs prerendering or needs core theme services directly, also ensure it can resolve `MazerModule`.

### 3. Update the client router to use `MainLayout`

In the client `Routes.razor`:

```razor
@using Starbender.Mazer
@using Volo.Abp.AspNetCore.Components.Web.Theming.Routing
@using Microsoft.Extensions.Options

@inject IOptions<AbpRouterOptions> RouterOptions

<Router AppAssembly="typeof(Program).Assembly" AdditionalAssemblies="RouterOptions.Value.AdditionalAssemblies">
    <Found Context="routeData">
        <AuthorizeRouteView RouteData="routeData" DefaultLayout="typeof(MainLayout)">
            <NotAuthorized>
                <RedirectToLogin />
            </NotAuthorized>
        </AuthorizeRouteView>
    </Found>
</Router>
```

### 4. Configure Blazor global assets on the host

Per ABP's WebAssembly bundling model, the host contributes to the generated `global.css` and `global.js` files by adding contributors to `BlazorMazerWebAssemblyBundles`.

Representative setup:

```csharp
private void ConfigureBundles()
{
    Configure<AbpBundlingOptions>(options =>
    {
        options.StyleBundles.Configure(
            MazerBundles.Styles.Global,
            bundle => { bundle.AddFiles("/global-styles.css"); }
        );

        options.ScriptBundles.Configure(
            MazerBundles.Scripts.Global,
            bundle => { bundle.AddFiles("/global-scripts.js"); }
        );
    });

    Configure<AbpBundlingOptions>(options =>
    {
        options.StyleBundles.Get(BlazorMazerWebAssemblyBundles.Styles.Global)
            .AddContributors(typeof(MyAppStyleBundleContributor));

        options.ScriptBundles.Get(BlazorMazerWebAssemblyBundles.Scripts.Global)
            .AddContributors(typeof(MyAppScriptBundleContributor));
    });
}
```

In the host `App.razor`, include the generated global asset files:

```razor
<!--ABP:Styles-->
<link href="global.css" rel="stylesheet" />
<!--/ABP:Styles-->

<!--ABP:Scripts-->
<script src="global.js"></script>
<!--/ABP:Scripts-->
```

### 5. Configure MudBlazor

In the client module:

```csharp
context.Services.AddMudTheme(context.Configuration.GetSection("MudTheme"));
```

If the host prerenders or renders MVC/account pages that depend on the same theme values, configure the same section there too.

## Blazor WebApp

Use this path for an ABP Blazor WebApp that mixes server and WebAssembly interactivity via `InteractiveAuto`.

### 1. Add the theme packages

Server project:

- `Starbender.Mazer.Mvc`
- `Starbender.Mazer.Server`
- `Starbender.Mazer.WebAssembly.Bundling`

Client project:

- `Starbender.Mazer.WebAssembly`

### 2. Replace the theme modules

Server module:

- Remove `AbpAspNetCoreMvcUiBasicThemeModule` if present.
- Remove `AbpAspNetCoreComponentsServerBasicThemeModule` if present.
- Remove `AbpAspNetCoreComponentsWebAssemblyBasicThemeBundlingModule` if present.
- Add `MazerMvcModule`.
- Add `MazerServerModule`.
- Add `MazerWebAssemblyBundlingModule`.

Client module:

- Remove `AbpAspNetCoreComponentsWebAssemblyBasicThemeModule` if present.
- Add `MazerWebAssemblyModule`.

### 3. Mark both modules as Blazor WebApp aware

In both server and client modules:

```csharp
PreConfigure<AbpAspNetCoreComponentsWebOptions>(options =>
{
    options.IsBlazorWebApp = true;
});
```

In the server project startup/module:

```csharp
context.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
```

### 4. Configure all three bundle surfaces

Blazor WebApp needs:

- `MazerBundles` for MVC / Razor Pages
- `BlazorMazerBundles` for server-rendered Blazor content
- `BlazorMazerWebAssemblyBundles` for generated `global.css` and `global.js`

Representative setup:

```csharp
private void ConfigureBundles()
{
    Configure<AbpBundlingOptions>(options =>
    {
        options.Parameters.InteractiveAuto = true;

        options.StyleBundles.Configure(
            MazerBundles.Styles.Global,
            bundle => { bundle.AddFiles("/global-styles.css"); }
        );

        options.ScriptBundles.Configure(
            MazerBundles.Scripts.Global,
            bundle => { bundle.AddFiles("/global-scripts.js"); }
        );

        options.StyleBundles.Configure(
            BlazorMazerBundles.Styles.Global,
            bundle => { bundle.AddFiles("/global-styles.css"); }
        );

        options.ScriptBundles.Configure(
            BlazorMazerBundles.Scripts.Global,
            bundle => { bundle.AddFiles("/global-scripts.js"); }
        );
    });

    Configure<AbpBundlingOptions>(options =>
    {
        options.StyleBundles.Get(BlazorMazerWebAssemblyBundles.Styles.Global)
            .AddContributors(typeof(MyAppStyleBundleContributor));

        options.ScriptBundles.Get(BlazorMazerWebAssemblyBundles.Scripts.Global)
            .AddContributors(typeof(MyAppScriptBundleContributor));
    });
}
```

### 5. Update `App.razor` for `InteractiveAuto`

Use `BlazorMazerBundles` for the server side and pass the generated WebAssembly files through `WebAssemblyStyleFiles` and `WebAssemblyScriptFiles`.

Representative structure:

```razor
<AbpStyles
    BundleName="@BlazorMazerBundles.Styles.Global"
    WebAssemblyStyleFiles="GlobalStyles"
    @rendermode="InteractiveAuto" />

<AbpScripts
    BundleName="@BlazorMazerBundles.Scripts.Global"
    WebAssemblyScriptFiles="GlobalScripts"
    @rendermode="InteractiveAuto" />

@code {
    private List<string> GlobalStyles => ["global.css"];
    private List<string> GlobalScripts => ["global.js"];
}
```

### 6. Update the client router and theme services

In the client `Routes.razor`, use `MainLayout` from Starbender.Mazer the same way as the Blazor WebAssembly migration.

Configure `AddMudTheme(...)` in both server and client modules so server-rendered and client-rendered parts share the same theme configuration.

## Verification Checklist

After migration, verify the following:

- The application starts without bundle or missing asset errors.
- Login, account, and other MVC / Razor Pages render with the Mazer layout.
- The main Blazor shell uses `MainLayout` from Starbender.Mazer.
- Menus, toolbars, theme mode, and notifications work in each UI mode.
- Application-specific CSS and JavaScript still load from the correct bundle locations.
- Any leftover Basic Theme usings, modules, or package references are removed or intentionally retained.
