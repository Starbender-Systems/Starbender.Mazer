# Understand the ABP installer package

`Starbender.Mazer.Installer` packages `Starbender.Mazer.abpmdl` and the `src/**/*.abppkg*` descriptors for ABP module installation. Its public module type is `Starbender.Mazer.AbpMudBlazorThemeInstallerModule` (the source filename is `MazerInstallerModule.cs`). The module depends on `AbpVirtualFileSystemModule` and registers the installer's embedded files.

Use this package when the consuming workflow needs ABP module metadata. Ordinary runtime theme integration uses the UI packages and modules in the [package guide](../packages.md). Referencing the installer does not select the MVC/Blazor theme, configure bundles, establish authentication, or populate an application's settings.

Before using an ABP installer workflow, inspect the descriptors for the selected library version and the application's established ABP tooling. This catalog does not prescribe an unverified CLI command or claim that descriptor automation handles every hosting model. Review the resulting project/package references, module dependencies, bundle constants, theme selection, and configuration against the appropriate [hosting recipe](README.md).

Acceptance: inspect the actual `.nupkg` for the module/package descriptors, review the application's generated diff, then build and smoke-test its host. The recipe checks verify the exported module type against source assemblies; they do not run the external ABP installer.
