# Starbender.Mazer
## Description
Starbender.Mazer is a UI theme toolkit for applications built with the [ABP Framework](https://abp.io). Its core Blazor package is published as `Starbender.Mazer.Blazor`, providing a cohesive Mazer-inspired visual style across ABP's supported UI models while combining [MudBlazor](https://mudblazor.com) components for Blazor experiences with matching MVC theme assets and layouts for server-rendered pages.

The goal of the project is to give ABP applications a consistent, modern look and feel without forcing each UI stack to be styled separately. It includes the modules, layouts, bundle contributors, toolbar integrations, and theme configuration hooks needed to use the same design language across MVC, Blazor Server, Blazor WebAssembly, and hybrid Blazor WebApp solutions.

## Features

- Supports all primary ABP UI types: MVC, Blazor Server, Blazor WebAssembly, and Blazor WebApp.
- Provides dedicated ABP modules for each UI surface, including `MazerMvcModule`, `MazerServerModule`, `MazerWebAssemblyModule`, and `MazerWebAssemblyBundlingModule`.
- Integrates with ABP's bundling system for MVC, Blazor Server, and Blazor WebAssembly global asset pipelines.
- Includes shared layouts and themed navigation for both MVC / Razor Pages and Blazor applications.
- Uses MudBlazor as the core Blazor component foundation and supports centralized `MudTheme` configuration.
- Adds toolbar, navigation, login, and account-page integration so ABP module pages can participate in the same theme.
- Supports hybrid solutions where MVC/account pages and Blazor UI need to coexist under one visual system.
- Includes demo applications for each supported UI model to use as migration and implementation references.

## Getting Started
Mazer supports Mvc, Blazor Server, Blazor WebAssembly (WASM) and hybrid Blazor WebApp models.
See the [Migration Guide](docs/Migration.md) for instructions on migrating your ABP UI application to Mazer.

## Community
- Contribution guide: [`CONTRIBUTING.md`](.github/CONTRIBUTING.md)
- Code of conduct: [`CODE_OF_CONDUCT.md`](.github/CODE_OF_CONDUCT.md)
- Security policy: [`SECURITY.md`](.github/SECURITY.md)
