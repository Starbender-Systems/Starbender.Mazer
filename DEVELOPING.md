# Developing Starbender.Mazer

Start with [AGENTS.md](AGENTS.md) and the pinned [.agent/VERSION](.agent/VERSION). This document contains repository-specific context; shared engineering rules live in [.agent/standards](.agent/standards/).

## Architecture and constraints

Mazer is a UI theme toolkit for ABP applications. [Starbender.Mazer.slnx](Starbender.Mazer.slnx) includes seven package projects and demos for MVC, Blazor Server, WebAssembly, and hybrid Blazor WebApp.

| Path | Responsibility |
| --- | --- |
| [src/Starbender.Mazer.Blazor](src/Starbender.Mazer.Blazor/) | Shared MudBlazor components, navigation, layouts, and theme configuration; start at `MazerModule.cs` and `MainLayout.razor`. |
| [src/Starbender.Mazer.Shared](src/Starbender.Mazer.Shared/) | Common assets and virtual file system integration. |
| [src/Starbender.Mazer.Mvc](src/Starbender.Mazer.Mvc/) | MVC/Razor Pages layouts, account pages, and bundle integration; start at `MazerMvcModule.cs`. |
| [src/Starbender.Mazer.Server](src/Starbender.Mazer.Server/) | Blazor Server integration; start at `MazerServerModule.cs`. |
| [src/Starbender.Mazer.WebAssembly](src/Starbender.Mazer.WebAssembly/) | Client-side Blazor integration and authentication; start at `MazerWebAssemblyModule.cs`. |
| [src/Starbender.Mazer.WebAssembly.Bundling](src/Starbender.Mazer.WebAssembly.Bundling/) | WebAssembly bundle contributors and shared asset integration. |
| [src/Starbender.Mazer.Installer](src/Starbender.Mazer.Installer/) | ABP module installation metadata and packaged descriptors. |
| [demo](demo/) | Integration applications and their local SQL Server/EF Core data setup. |
| [docs/consumers/installation.md](docs/consumers/installation.md) | Canonical package installation and Basic/LeptonX/Lite theme replacement guide; `docs/Migration.md` preserves earlier entry links. |
| [docs/consumers](docs/consumers/index.md) | Searchable package/capability catalog and integration recipes assembled into the consumer plugin. |

Keep theme/module APIs, asset paths, authentication/navigation behavior, and ABP bundling consistent across the supported UI models. Check the affected demos when changing shared components or assets.

Package projects use [src/Directory.Build.props](src/Directory.Build.props), which imports the root [Directory.Build.props](Directory.Build.props). Central package versions for `src/` live in [Directory.Packages.props](Directory.Packages.props); demos have their own [build properties](demo/Directory.Build.props) and [package versions](demo/Directory.Packages.props). Both sets must be considered for dependency changes. [configureawait.props](configureawait.props) adds Fody references in Release builds where imported.

The source package baseline declares Mazer `1.1.0`, ABP `10.0.2`, and MudBlazor `8.6.0`. Inspect the version files for the current values; avoid unrelated dependency or release-version changes during maintenance. Demo `package.json` and `yarn.lock` files govern their client libraries. Generated `bin/`, `obj/`, and `artifacts/` output is ignored; update client libraries through their tooling rather than hand-editing vendored `wwwroot/libs/` files.

The root `VersionPrefix` controls all seven source package versions and their assembly/file versions. Keep the consumer catalog, installation examples, package readmes, and [release notes](docs/consumers/release-notes.md) aligned when preparing a library release. The consumer plugin has an independent version; update it when its bundled guidance changes. Demo application/frontend versions and third-party package versions are independent of the library version.

## Environment and commands

Run the following commands from the repository root unless stated otherwise.

- .NET 10 SDK: all projects target `net10.0`; CI selects `10.0.x`. There is no `global.json` pin.
- Demo client-library setup additionally uses Node.js, Yarn, and the ABP CLI (`abp`). See each demo README and its lockfile for the existing setup; this repository does not pin those tool versions globally.
- Demo runtime setup requires SQL Server and local development HTTPS configuration. Building the packages does not require migrating a database.
- Shared snapshot tooling requires Python 3.11+ and a local `engineering-standards` checkout. On Windows, `py -3.12` selects a suitable interpreter even when `python` resolves to an older version.

Restore and build, matching the [existing CI workflow](.github/workflows/build-and-publish-packages.yml):

```powershell
dotnet --version
dotnet restore Starbender.Mazer.slnx
dotnet build Starbender.Mazer.slnx --configuration Release --no-restore
```

For a focused package build (after restore):

```powershell
dotnet build src/Starbender.Mazer.Blazor/Starbender.Mazer.Blazor.csproj --configuration Release --no-restore
```

For a scoped formatting check (replace the example file with a changed C# file):

```powershell
dotnet format Starbender.Mazer.slnx --verify-no-changes --no-restore --include src/Starbender.Mazer.Blazor/MazerModule.cs
```

Follow [.editorconfig](.editorconfig). Formatting is not currently enforced by a dedicated CI step; do not run an unscoped formatting rewrite as part of unrelated work.

No automated test projects or test runner configuration are currently present. A successful `dotnet test` with no discovered tests is not test evidence. Build the affected packages and demos, then smoke-test affected UI behavior: layout, navigation, login/logout, localization, theme switching, and asset loading as applicable. Report which hosts and flows were exercised and any runtime prerequisites that prevented checks.

The consumer guides have a separate validation executable, outside the solution. With Python 3.11+ and .NET 10, run:

```powershell
py -3.12 tools/check_consumer_recipes.py
py -3.12 tools/consumer_tooling.py validate . --inventory artifacts/consumer/inventory.json
```

This compiles marked C#/Razor examples throughout the consumer guides against source packages, checks theme configuration/serialization (including absent settings and invalid JSON) and explicit default selection, and emits an exported-type inventory for catalog validation. It does not run an application or verify browser/authentication behavior. See [consumer-tooling maintenance](docs/consumers/maintenance.md) for packaging, version updates, and the remaining runtime checks. Maintain the catalog, recipes, and plugin version together when consumer-facing capabilities change.

## Running demos and working with data

Read the [Server](demo/Server/README.md), [MVC](demo/MvcUi/README.md), [WebAssembly](demo/WebAssembly/README.md), or [WebApp](demo/WebApp/README.md) instructions before running that model. From the relevant demo directory (for example `demo/Server`), run `abp install-libs` when client libraries need initialization or refresh. Review the resulting lockfile and asset changes.

Configure that demo's `ConnectionStrings:Default` for a local development SQL Server database using environment variables, user secrets where configured, or ignored `appsettings.Development.json`. Keep credentials out of tracked instructions and configuration. Use the demo's launch settings for its URLs and development environment.

The following example migrates and seeds the Server demo's configured database. Run it only after verifying the connection targets the intended local database:

```powershell
dotnet run --project demo/Server/Mazer.ServerDemo/Mazer.ServerDemo.csproj -- --migrate-database
```

The other host projects expose the same `--migrate-database` application flag. EF models, migration services, and committed migrations live under each host's `Data/` and `Migrations/` directories. Follow the [EF migration workflow](.agent/workflows/ef-migration.md) for schema changes. Inspect the generated SQL and preserve recoverable local data before applying migrations. The `etc/scripts/initialize-solution.ps1` scripts also install libraries, migrate/seed data, and create certificates; inspect their targets before use.

Launch the chosen model from the repository root after its setup:

```powershell
dotnet run --project demo/Server/Mazer.ServerDemo/Mazer.ServerDemo.csproj
dotnet run --project demo/MvcUi/Mazer.MvcDemo/Mazer.MvcDemo.csproj
dotnet run --project demo/WebApp/Mazer.WebAppDemo/Mazer.WebAppDemo.csproj
```

For the standalone WebAssembly model, run the API host and client in separate terminals:

```powershell
dotnet run --project demo/WebAssembly/Mazer.WebAssemblyDemo.Host/Mazer.WebAssemblyDemo.Host.csproj
dotnet run --project demo/WebAssembly/Mazer.WebAssemblyDemo.Blazor/Mazer.WebAssemblyDemo.Blazor.csproj
```

## Ownership and path instructions

[CODEOWNERS](.github/CODEOWNERS) assigns the repository to `@alvahdean`. The [contribution guide](.github/CONTRIBUTING.md) defines the fork-and-PR process, `development` integration branch, and promotion to `main`. The existing package workflow publishes prerelease packages after merged PRs into `development` and stable packages after merged PRs into `main`; merges and publication follow that process.

- [.NET path instructions](.github/instructions/dotnet.instructions.md) cover C#, Razor/Blazor, project/solution files, and build/dependency properties.
- [Database path instructions](.github/instructions/database.instructions.md) cover demo EF models, migration services, migrations, and SQL.
- [Test path instructions](.github/instructions/tests.instructions.md) cover future test projects; there is no test suite yet.

Future decisions: select any pinned Node/Yarn/ABP CLI versions and define an automated test strategy. These are not established requirements in this bootstrap.

## Shared snapshot provenance and updates

The initial snapshot uses local `engineering-standards` release `0.1.0` at commit `758a498d2822f076478dfadd13a86201c182c65a`. The source working tree was clean at adoption; this records a source revision, not a claim that a release tag was published.

[.agent/manifest.yaml](.agent/manifest.yaml) selects the general software and .NET standards, software workflows including EF migration, and .NET/repository-maintenance skills. Hardware and Python guidance are not selected. Synchronization includes any transitive shared links and records exact content hashes in [.agent/sync-state.json](.agent/sync-state.json).

[.gitattributes](.gitattributes) keeps `.agent/` files at LF line endings so Windows checkouts preserve the synchronized bytes and hashes.

With the matching reviewed standards checkout at `../engineering-standards`, run from this repository root:

```powershell
py -3.12 ../engineering-standards/tools/validate_repository.py
py -3.12 ../engineering-standards/tools/sync_standards.py .
py -3.12 ../engineering-standards/tools/sync_standards.py .
py -3.12 ../engineering-standards/tools/sync_standards.py . --check
git diff --check
```

The repeat synchronization should report zero changes; `--check` should report no drift. On other platforms, use a Python 3.11+ executable in place of `py -3.12`. The source tools operate offline; a sibling checkout is needed only for validation or updates, not for reading the committed snapshot.

For upgrades, obtain a reviewed source checkout, update the manifest release/selections, synchronize, check drift and local links, and review the complete diff. Include the source commit, compatibility notes, and validation in the PR. Keep `AGENTS.md`, `CLAUDE.md`, `DEVELOPING.md`, `.github/`, and the request manifest locally owned; do not edit files in the synchronized shared directories. To recover, restore the prior committed manifest, snapshot (including hashes and pin), and adapters together, then check against its recorded source revision.
