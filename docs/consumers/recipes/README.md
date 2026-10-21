# Integration recipes

These recipes target the **1.0.4 source API**, ABP **10.0.2**, MudBlazor **8.6.0**, and **net10.0**. Check [adoption](../adoption.md) before using them with installed packages.

- [MVC / Razor Pages](mvc.md)
- [Shared Blazor layout and router](blazor-layout.md)
- [Blazor Server](blazor-server.md)
- [WebAssembly client and host](blazor-webassembly.md)
- [Blazor WebApp / InteractiveAuto](blazor-webapp.md)
- [Assets and bundling](assets.md)
- [Theme configuration, serialization and mode](theming.md)
- [Navigation, toolbars and localization](navigation.md)
- [Messages, notifications and page progress](feedback.md)
- [Authentication UI](authentication.md)
- [Installer metadata](installer.md)

Code fences marked `recipe-check` are compiled (C#/Razor) or loaded as configuration (JSON) by the library's recipe-check command documented in [maintenance](../maintenance.md). They are integration fragments, not complete application startup or authorization implementations. The examples remain bundled Markdown; consumers do not need the compilation tool to read them. Browser and application acceptance still need to run in the real consumer.
