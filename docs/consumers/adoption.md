# Adopt Starbender.Mazer

Read the application's AGENTS.md, setup instructions, and applicable path rules first. Identify its actual model: MVC, interactive Server, WebAssembly, or WebApp with InteractiveAuto. A client plus server project does not by itself establish the render/authentication model.

## Establish version evidence

The catalog's seven Mazer packages evaluate to **1.0.4** at the [inspected source revision](source-evidence.md). All target **net10.0** and use ABP **10.0.2**; the source MudBlazor version is **8.6.0**. This guide does not assert package-feed availability or compatibility with ABP 10.1+, newer MudBlazor majors, or every earlier Mazer build.

Inspect application `PackageReference`, `Directory.Packages.props`, lockfiles, `obj/project.assets.json`, and any switched `ProjectReference`. A useful command, after the application's normal restore, is:

```text
dotnet list path/to/Application.csproj package --include-transitive
```

For switched source, record the checkout commit and evaluated project versions. Use the application's package-management conventions; do not upgrade dependencies merely to make a recipe available. If versions differ, inspect that version's actual signatures and assets before adapting examples. A catalog entry is discovery evidence, not proof that the installed binary contains it.

## Install or replace a theme

Use the canonical [installation and theme replacement guide](installation.md) for Basic Theme, LeptonX, and LeptonX Lite migrations. It covers project-specific package installation/removal, central versions, ABP modules, explicit defaults, custom layouts/assets, and acceptance checks. The linked hosting recipes supply code fragments; keep application-owned routes, identity, authorization, localization, and business services intact.

## Verify the selected integration

Run the consuming application's established checks and the canonical guide's [runtime acceptance steps](installation.md#7-restore-build-and-verify-the-migration). Package declarations and compiled fragments do not establish installed-binary or live UI compatibility.

In an extracted plugin, `provenance.json` records the bundle's actual commit/ref, declared versions, input hashes, optional inventory and preview status. The source checkout's documentation links remain useful evidence but are not runtime prerequisites for the bundle. Preview output is not a published plugin. The [maintenance guide](maintenance.md) separates recipe compilation and metadata checks from browser/application acceptance.
