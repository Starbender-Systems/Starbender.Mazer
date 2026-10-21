# Load and extend theme assets

Use the hosting recipe first: [MVC](mvc.md), [Server](blazor-server.md), [WebAssembly](blazor-webassembly.md), or [WebApp](blazor-webapp.md). Their bundle identifiers are different even though their constants have similar names.

| Host | Constants namespace and type | Global styles/scripts ID |
| --- | --- | --- |
| MVC | `Starbender.Mazer.Mvc.Bundling.MazerBundles` | `Mazer.Global` |
| Server | `Starbender.Mazer.Server.Bundling.BlazorMazerBundles` | `Blazor.Global` |
| WebAssembly | `Starbender.Mazer.WebAssembly.Bundling.BlazorMazerWebAssemblyBundles` | `BlazorWebAssembly.Global` |

Add application styles and scripts with `Configure<AbpBundlingOptions>` and `StyleBundles.Configure` / `ScriptBundles.Configure`, as in the MVC recipe. Keep existing ABP contributors and application files. In WebApp, configure `Parameters.InteractiveAuto = true` and retain both the server bundles and generated client `global.css` / `global.js` references.

`Starbender.Mazer.MazerAssetFiles` in Shared defines these paths without a leading slash:

| Constant | Static asset path |
| --- | --- |
| `MudBlazorCss` | `_content/MudBlazor/MudBlazor.min.css` |
| `ThemeCss` | `_content/Starbender.Mazer.Blazor/mazer.css` |
| `MvcLayoutCss` | `_content/Starbender.Mazer.Mvc/layout.css` |
| `GoogleFontCss` | `_content/Starbender.Mazer.Mvc/googlefonts.css` |
| `MudBlazorJs` | `_content/MudBlazor/MudBlazor.min.js` |
| `ThemeModeJs` | `_content/Starbender.Mazer.Blazor/mazer-mode.js` |
| `MvcLayoutJs` | `_content/Starbender.Mazer.Mvc/layout.js` |

Server contributors add MudBlazor CSS/JS, Mazer CSS, and theme-mode JS with root-relative paths. MVC adds its layout CSS/JS and font CSS on top of ABP standard bundles. WebAssembly contributors include the shared, Blazor, and MVC assets using paths suitable for client bundle generation. A project reference alone does not activate these contributors; register the hosting modules described in [packages](../packages.md).

For a missing style or script, inspect the rendered document and network responses, confirm the correct host bundle is rendered, and verify the package's static web assets reach the deployed host. Preserve the application's base path, culture/RTL assets, isolated CSS, and script order. Do not add a second MudBlazor script, theme-mode script, or Blazor boot script to compensate for a missing module. Run the host's existing ABP client-library process when required by that application.

Acceptance: no asset 404s, one Blazor boot script for the selected hosting model, working MudBlazor popovers/dialogs, MVC navigation where present, and matching light/dark styling after a reload. A successful C# build does not verify deployed static assets.
