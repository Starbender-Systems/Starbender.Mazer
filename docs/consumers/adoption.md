# Adopt Starbender.Mazer

Read the application's AGENTS.md, setup instructions, and applicable path rules first. Identify its actual model: MVC, interactive Server, WebAssembly, or WebApp with InteractiveAuto. A client plus server project does not by itself establish the render/authentication model.

## Establish version evidence

The catalog's seven Mazer packages evaluate to **1.0.4** at the [inspected source revision](source-evidence.md). All target **net10.0** and use ABP **10.0.2**; the source MudBlazor version is **8.6.0**. This guide does not assert package-feed availability or compatibility with ABP 10.1+, newer MudBlazor majors, or every earlier Mazer build.

Inspect application `PackageReference`, `Directory.Packages.props`, lockfiles, `obj/project.assets.json`, and any switched `ProjectReference`. A useful command, after the application's normal restore, is:

```text
dotnet list path/to/Application.csproj package --include-transitive
```

For switched source, record the checkout commit and evaluated project versions. Use the application's package-management conventions; do not upgrade dependencies merely to make a recipe available. If versions differ, inspect that version's actual signatures and assets before adapting examples. A catalog entry is discovery evidence, not proof that the installed binary contains it.

## Integrate within the application

Choose the smallest [package set](packages.md) and [recipe](recipes/README.md). The module examples are additive `AbpModule` adapters: add the adapter to the existing application's `[DependsOn]`, or merge its relevant methods into the owning module. Preserve existing identity, persistence, localization, HTTP clients and endpoints. Replace the outgoing UI theme modules deliberately; both MVC and Blazor theming only select Mazer automatically when the respective `DefaultThemeName` is null.

Examples use the inspected source APIs. Application CSS/JS filenames, module names, localization keys, menu permission names and routes are examples to replace with real application values. They are not additional library APIs. Files added to bundles must exist in the application's static assets or virtual file system.

Use [MainLayout](recipes/blazor-layout.md) once per routed page hierarchy; it already provides MudBlazor providers and Mazer feedback listeners. Configure palettes in each rendering host/client and preserve the correct [authentication](recipes/authentication.md) flow. Menu visibility and `AuthorizeRouteView` do not replace API authorization.

## Validate the actual integration

Run the consuming application's established restore/build/tests. Exercise the affected routes in its real host: unauthenticated and authenticated navigation, login/logout and return URL, localized menus/RTL where supported, responsive navigation, light/dark mode, feedback UI, asset requests, and full reloads. InteractiveAuto additionally needs initial server rendering and subsequent client execution checked independently.

In an extracted plugin, `provenance.json` records the bundle's actual commit/ref, declared versions, input hashes, optional inventory and preview status. The source checkout's documentation links remain useful evidence but are not runtime prerequisites for the bundle. Preview output is not a published plugin. The [maintenance guide](maintenance.md) separates recipe compilation and metadata checks from browser/application acceptance.
