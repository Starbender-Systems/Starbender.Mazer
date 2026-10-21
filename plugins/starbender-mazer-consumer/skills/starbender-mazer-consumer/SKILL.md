---
name: starbender-mazer-consumer
description: "Integrate Starbender.Mazer ABP themes in MVC, Blazor Server, WebAssembly, and Blazor WebApp consumers. Use for Mazer adoption, layouts, assets, palettes, navigation, authentication UI, and feedback services. Excludes maintenance of Mazer itself and unrelated application work."
---

# Integrate Starbender.Mazer

Read the consuming repository's AGENTS.md and applicable path instructions first. This plugin supplies library capability guidance; the application owns its engineering rules and product decisions.

Use the bundled [catalog](references/index.md), [discovery metadata](references/catalog.json), and [package guide](references/packages.md) to find a capability by application need. Read only relevant recipes. Use [adoption](references/adoption.md) for version discovery and configuration checks. The source baseline is Mazer 1.0.4 / .NET 10 / ABP 10.0.2 / MudBlazor 8.6.0; verify the consuming application's resolved versions and feed availability before changing references.

Start with the appropriate [hosting recipe](references/recipes/README.md), then follow links for the requested feature. MVC and Blazor use different theme names and MVC, Server, and WebAssembly use different bundle IDs. WebApp needs configuration in both rendering projects. Preserve the application's existing authentication, startup, routing, and assets when adding the integration.

1. Identify the affected application projects and their actual resolved library versions, including central version files, lockfiles, transitive dependencies, and switched source references where applicable.
2. Search existing application integrations, then catalog search terms and owning packages. Check relevant source or generated API inventory when available. Bundles may include `references/api/inventory.json`; `references/provenance.json` records the source revision and declared package versions.
3. Verify signatures, prerequisites, service lifetimes, configuration, and compatibility with the installed version before using an API. Catalog declarations alone do not prove runtime or binary compatibility. If version evidence is unavailable, report what remains unverified.
4. Prefer direct reuse or composition. Keep application authorization, business policy, endpoints, and data mappings in application adapters. If local implementation is needed, identify the specific capability gap and the shared APIs investigated.
5. Run the application's established validation for the affected integration and report results and remaining runtime checks. Do not install or upgrade packages or adopt engineering standards as an incidental part of integration.

In the library authoring repository, canonical references live under `docs/consumers/`; the bundler assembles the relative `references/` tree. Review and test the packaged skill after extraction into an unrelated directory.
