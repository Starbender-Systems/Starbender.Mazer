# Release notes

## Consumer plugin 0.3.0 (unreleased)

The consumer bundle adds generated Claude Code plugin metadata and a marketplace shared by Claude Code and GitHub Copilot CLI. OpenAI/Codex packaging is preserved. All hosts use one skill and one set of recipes; Copilot IDE and cloud users can import that complete skill into their application's `.github/skills` directory. See [coding-agent setup](agent-setup.md) for installation and verification.

Host metadata is derived from the canonical plugin name, version, description, repository, author, and marketplace name. This release changes no Mazer packages or runtime contracts. Bundle tests cover extraction, host metadata, portable links, provenance hashes, and standalone repository-skill use; live host acceptance remains a separate check.

## 1.1.0 (unreleased)

This source checkout prepares the next Mazer minor version, advancing the library from **1.0.4 to 1.1.0**. All seven packages in the [package map](packages.md) share version **1.1.0**; assembly and file versions advance to **1.1.0.0**. This note does not assert that packages or a release tag have been published.

The compatibility baseline remains **.NET 10**, **ABP 10.0.2**, and **MudBlazor 8.6.0**. This version preparation introduces no runtime API, module, theme-name, asset-path, database, or dependency-baseline changes. It aligns package metadata, NuGet readmes, catalog declarations, installation examples, and consumer guidance. Consumer plugin **0.2.2** carries the updated guidance and has its own version lifecycle.

### Upgrade and validation

After the selected 1.1.0 package build is available in your configured feed, align all directly referenced Mazer packages across server, client, and account/AuthServer projects. Use your existing central package-management or project-reference conventions and review regenerated lockfiles. Development publishing produces versions such as `1.1.0-prerelease-<run>`; use the actual available version consistently rather than assuming the stable package already exists.

For an application already using 1.0.4, this version bump alone requires no theme migration or database migration. Rebuild and run the application's established checks, especially when assembly versions are pinned explicitly. For adoption or replacing Basic Theme, LeptonX, or LeptonX Lite, follow the [canonical installation guide](installation.md).

Repository validation evaluates all package/assembly versions, compiles consumer examples, checks theme configuration/selection, and validates the catalog against a generated source inventory. Local package/preview assembly is separate from publication. Browser rendering and authentication still require the application's runtime acceptance checks; successful compilation does not establish them.

### Publication and recovery

Publication follows the existing development-to-main release process. Merging and publishing are separate authorized operations; preparing these files does not perform either operation. Bundle provenance records the actual source commit; the earlier implementation links in [source evidence](source-evidence.md) retain their historical 1.0.4 context.

To undo an application upgrade, restore its previous Mazer references/central versions and lockfiles together, then restore and rebuild. Do not overwrite an immutable published package or move an existing release tag.
