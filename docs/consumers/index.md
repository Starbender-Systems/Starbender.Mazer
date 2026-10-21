# Starbender.Mazer consumer catalog

Use the [coding-agent setup guide](agent-setup.md) to load the shared consumer skill in OpenAI/Codex, Claude Code, or GitHub Copilot.

Discover Mazer's existing ABP theme integration before building a separate application shell, palette serializer, menu renderer, or feedback bridge. The catalog describes **seven source packages at 1.1.1**, targeting **net10.0**, ABP **10.0.2**, and MudBlazor **8.6.0**. These are evaluated source declarations, not a claim that 1.1.1 is available from a package feed. Start with [adoption and compatibility](adoption.md) and the [package map](packages.md).

| Application need | Recipe |
| --- | --- |
| Install Mazer; replace Basic Theme, LeptonX, or LeptonX Lite | [Installation and theme replacement](installation.md) |
| MVC / Razor Pages, account and application layouts | [MVC](recipes/mvc.md) |
| Shared MudBlazor shell, router, providers, layout hooks | [Blazor layout](recipes/blazor-layout.md) |
| Interactive Server UI | [Blazor Server](recipes/blazor-server.md) |
| WebAssembly client and host | [Blazor WebAssembly](recipes/blazor-webassembly.md) |
| InteractiveAuto / hybrid rendering | [Blazor WebApp](recipes/blazor-webapp.md) |
| Bundle IDs, CSS/JS loading, static asset failures | [Assets](recipes/assets.md) |
| Branding, palettes, JSON themes, light/dark mode | [Theming](recipes/theming.md) |
| ABP menus, toolbars, localization and permission-aware UI | [Navigation](recipes/navigation.md) |
| Messages, confirmations, notifications, snackbar distinction and progress | [Feedback](recipes/feedback.md) |
| Login/logout integration and host account endpoints | [Authentication](recipes/authentication.md) |
| ABP installation descriptors | [Installer metadata](recipes/installer.md) |

[catalog.json](catalog.json) supplies searchable routes, owning packages, and public type IDs. Read the relevant recipe rather than loading every page. [Source evidence](source-evidence.md) pins the inspected implementation and demo paths. The catalog is curated; it is not a complete API inventory. Plugin **0.3.1** has its own version. `library.ref` remains `HEAD` so a committed bundle records the actual source revision; inspect its generated provenance before treating an API as available.

Application authorization, identity providers, persistence, endpoints, render modes and business rules remain application responsibilities. See [maintenance](maintenance.md) for catalog checks, recipe compilation, preview packaging, and runtime acceptance limits.
