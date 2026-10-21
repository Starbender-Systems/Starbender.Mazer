# Starbender.Mazer packages

Every package below evaluates to **1.0.4**, `net10.0`, and `IsPackable=true` in the inspected source. ABP package references are **10.0.2** and MudBlazor is **8.6.0**. Package availability and a consuming application's resolved dependencies must be checked separately; see [adoption](adoption.md).

| Package | Entry point / responsibility | Direct Mazer project dependencies |
| --- | --- | --- |
| `Starbender.Mazer.Blazor` | `Starbender.Mazer.MazerModule`, `MainLayout`, Blazor `StarbenderTheme`, theme manager, navigation, ABP feedback service replacements. `MazerModule` registers MudBlazor services. | None |
| `Starbender.Mazer.Shared` | `MazerSharedModule`, `MazerAssetFiles`, `Extensions.ServiceCollectionExtensions.AddMudTheme`, `Utilities.MudThemeJsonSerializer`. Configuration and common embedded assets. | None |
| `Starbender.Mazer.Mvc` | `Starbender.Mazer.Mvc.MazerMvcModule`, MVC `StarbenderTheme`, Razor layouts/components and `MazerBundles`. | Shared |
| `Starbender.Mazer.Server` | `Starbender.Mazer.Server.MazerServerModule`, Server toolbar/login/language components and `Server.Bundling.BlazorMazerBundles`. | Shared, Blazor |
| `Starbender.Mazer.WebAssembly` | `Starbender.Mazer.WebAssembly.MazerWebAssemblyModule`, authentication route/reconfiguration and client toolbar components. | WebAssembly.Bundling, Blazor |
| `Starbender.Mazer.WebAssembly.Bundling` | `Starbender.Mazer.WebAssembly.Bundling.MazerWebAssemblyBundlingModule`, asset contributors and `BlazorMazerWebAssemblyBundles`. | Shared |
| `Starbender.Mazer.Installer` | `Starbender.Mazer.AbpMudBlazorThemeInstallerModule` and packaged ABP descriptors. It does not select a UI theme by itself. | None |

Project references describe package dependency ownership; they do **not** automatically activate an ABP module. Inspect `[DependsOn]` as well. For example, Server references Shared as a project but does not declare `MazerSharedModule` in its module dependencies; the demos explicitly call `AddMudTheme`.

## Choose by UI model

| Consumer project | Typical direct references and module dependencies |
| --- | --- |
| MVC web app | Mvc / `MazerMvcModule` |
| Server-rendered Blazor app with account pages | Server and Mvc / `MazerServerModule`, `MazerMvcModule` |
| WebAssembly client | WebAssembly / `MazerWebAssemblyModule` |
| WebAssembly host | Mvc and WebAssembly.Bundling; add Blazor when directly configuring its services / `MazerMvcModule`, `MazerWebAssemblyBundlingModule`; the source demo also depends on `MazerModule` |
| InteractiveAuto server | Server, Mvc, WebAssembly.Bundling / corresponding three modules |
| InteractiveAuto client | WebAssembly / `MazerWebAssemblyModule` |

Use the matching [integration recipe](recipes/README.md). Preserve unrelated ABP modules and application registration. Remove the previous theme's module dependencies only where Mazer replaces them; keep required ABP shared infrastructure and any Blazorise providers still used by other module pages. Configure the same `MudTheme` values in every rendering process that needs them, and load assets through the correct [bundle family](recipes/assets.md).
