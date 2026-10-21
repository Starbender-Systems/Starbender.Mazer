# Use the consumer skill with coding agents

Consumer plugin **0.3.1** packages one shared skill and the same integration guides for OpenAI/Codex, Claude Code, and GitHub Copilot. It supplies documentation, not an MCP server or an OpenAI API integration. Each host still requires its own installation and access. Follow the consuming application's agent instructions, including AGENTS.md, CLAUDE.md, or .github/copilot-instructions.md where applicable.

## Obtain a complete bundle

Use an extracted consumer marketplace ZIP, not the raw `plugins/starbender-mazer-consumer` directory from a source checkout: the bundler adds the references and host metadata. See [maintenance](maintenance.md) for release packaging, provenance, checksums, and publication status. A local preview can be built from the Mazer repository root with Python 3.11+; the output directory must be new and outside the repository:

```powershell
py -3.12 tools/consumer_tooling.py bundle . --output ../mazer-agent-preview --allow-dirty-preview
Expand-Archive ../mazer-agent-preview/starbender-mazer-consumer-0.3.1-preview.zip ../mazer-agent-preview/extracted
```

Python 3.11+ on other platforms can run the same bundler with `python`; extract the ZIP using your platform's archive tool. API inventory is optional for this preview; the maintenance workflow includes it. A preview is for local review and does not establish publication.

The extracted marketplace has this layout:

```text
.agents/plugins/marketplace.json             # OpenAI/Codex marketplace
.claude-plugin/marketplace.json              # Claude Code and Copilot CLI marketplace
plugins/starbender-mazer-consumer/
  plugin.json                               # Shared Agent Plugins manifest
  .claude-plugin/plugin.json                 # Generated Claude Code manifest
  skills/starbender-mazer-consumer/
    SKILL.md                                # One shared workflow
    references/                             # Catalog, guides, recipes, provenance
```

The host adapters are generated from `plugin.json` and the existing marketplace name. There are no separately maintained Claude or Copilot recipes. All commands below are examples using the preview path above; replace it with the absolute path to your extracted marketplace when running from another directory.

## OpenAI/Codex

Use the host's plugin installation flow with the extracted `.agents/plugins/marketplace.json`. This preserves the existing marketplace name, plugin identity, policy, and OpenAI interface metadata. Enable `starbender-mazer-consumer` in the consuming chat. Bundle generation does not install or enable it.

## Claude Code

With Claude Code installed, register the extracted marketplace and install the plugin:

```text
claude plugin validate ../mazer-agent-preview/extracted
claude plugin validate ../mazer-agent-preview/extracted/plugins/starbender-mazer-consumer
claude plugin marketplace add ../mazer-agent-preview/extracted
claude plugin install starbender-mazer-consumer@starbender-mazer-tooling
```

Start a new session in the consuming application. Invoke `/starbender-mazer-consumer:starbender-mazer-consumer` with your integration request, or ask Claude to use the Mazer consumer skill. Use `claude plugin list` to check installation. The manifest lives in `.claude-plugin/`; the shared `skills/` directory stays at the plugin root, as required by the [Claude Code manifest reference](https://code.claude.com/docs/en/plugins-reference). The commands follow the [marketplace setup guide](https://code.claude.com/docs/en/plugin-marketplaces).

## GitHub Copilot CLI

With Copilot CLI installed, register the same extracted marketplace and install:

```text
copilot plugin marketplace add ../mazer-agent-preview/extracted
copilot plugin install starbender-mazer-consumer@starbender-mazer-tooling
copilot plugin list
```

Start a new session in the consuming application and ask Copilot to use `starbender-mazer-consumer` for the integration. Copilot CLI discovers `.claude-plugin/marketplace.json` and loads the root Agent Plugins manifest and `skills/` directory. See the [Copilot CLI plugin reference](https://docs.github.com/en/copilot/reference/copilot-cli-reference/cli-plugin-reference).

## GitHub Copilot in IDEs and on GitHub

For clients supporting repository agent skills, copy the complete extracted `plugins/starbender-mazer-consumer/skills/starbender-mazer-consumer/` directory into the consuming repository as `.github/skills/starbender-mazer-consumer/`. Copy both `SKILL.md` and `references/`; the links must stay relative. Review any existing skill before replacing it. Commit the directory if a cloud session needs it. This imports a snapshot; update the entire directory from a later bundle to receive new guidance.

Use an IDE version that supports agent skills, start an agent session, and ask it to use the Mazer consumer skill. A CLI plugin install does not install a repository skill for a cloud session. See GitHub's [agent skill support and storage locations](https://docs.github.com/en/copilot/concepts/customizations/agent-skills).

## Verify and remove

Native verification on Windows ARM64 on **2026-10-21** used the extracted 0.3.0 preview with its API inventory and isolated CLI profiles:

Those recorded results apply to plugin 0.3.0. Plugin 0.3.1 updates the declared source baseline to Mazer 1.1.1; its integration workflow is unchanged. Packaging checks for the current bundle do not repeat the recorded native CLI tests.

| Client | Installation and discovery | Live use |
| --- | --- | --- |
| Claude Code 2.1.306 | Marketplace and plugin validation passed; marketplace registration and plugin installation succeeded; `plugin details` listed one consumer skill; a real session registered `starbender-mazer-consumer:starbender-mazer-consumer` | Passed after account sign-in: invoked the namespaced skill, opened the bundled catalog, WebApp recipe, and assets recipe, and answered the integration question using only skill/read tools |
| Copilot CLI 1.0.101 | Marketplace registration and plugin installation succeeded; `plugin list --json` and `skill list --json` listed the enabled consumer plugin/skill | Passed: invoked the skill, opened the bundled catalog, WebApp recipe, and assets recipe, and answered the integration question with zero file changes |

Both read-only checks verified the InteractiveAuto server/client package-module split, all three bundle IDs, generated client assets, cold/return visit acceptance steps, and the distinction between the declared source baseline and installed-package compatibility. Their available tools were file reading, search, and skill invocation; shell execution and file writes were disabled. Copilot's built-in MCP servers were disabled. The Windows checks used native executables and single-line prompts. An inherited classic `GITHUB_TOKEN` had to be omitted from the Copilot process so the CLI could use an existing supported sign-in; the token was omitted only for that process. Claude required the account holder to sign in and resolve an account terms-acceptance requirement before its live test succeeded.

Claude's normal local validation passed. Its additional plugin-directory submission checks reported missing README/license/icon material in the extracted plugin; those checks did not prevent local installation or skill discovery. These results do not establish public directory eligibility, cloud/IDE execution, or application runtime behavior.

Check that the host discovers the skill and can open `references/index.md` and the appropriate installation or hosting recipe. Confirm it checks application package versions and follows the application's existing instructions. Then run the [integration acceptance steps](installation.md#7-restore-build-and-verify-the-migration) for any application changes. Packaging checks alone do not verify agent discovery, browser rendering, or authentication.

To remove a CLI installation, use `claude plugin uninstall starbender-mazer-consumer@starbender-mazer-tooling` or `copilot plugin uninstall starbender-mazer-consumer`. For a copied repository skill, remove only its imported directory through the application's normal review process. Do not enable both a copied skill and a plugin supplying the same skill in one session.
