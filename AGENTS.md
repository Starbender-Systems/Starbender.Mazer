# Starbender.Mazer agent instructions

This file is owned by this repository. Read [DEVELOPING.md](DEVELOPING.md) for architecture, commands, and constraints, then the applicable material in [.agent/standards](.agent/standards/). The shared snapshot pin is [.agent/VERSION](.agent/VERSION).

## Instruction precedence

<!-- authority: template.instruction-precedence -->

Within repository-provided guidance, resolve conflicts in this order:

1. Explicit user task instructions.
2. Repository-specific instructions.
3. Path-specific instructions.
4. Shared engineering standards.
5. General agent defaults.

Path instructions refine work in their scope but cannot silently override a repository-wide constraint. Surface contradictions and resolve them with task context or the repository maintainer. Platform system instructions and tool permissions remain in force.

Agent-specific files may add native behavior but must not silently override shared engineering standards. Workflows and skills apply to the selected task and reference shared rules; they are not a higher precedence layer.

## Load applicable guidance

Start with [general](.agent/standards/general.md), then select standards for the affected concerns and stack. Use a procedure from [.agent/workflows](.agent/workflows/) when it matches the task.

Read the selected skill explicitly; `.agent/skills/` does not guarantee native discovery:

- [.NET engineering](.agent/skills/sb-dotnet-engineering/SKILL.md) for C#, Razor/Blazor, ABP integration, and EF changes.
- [Repository maintenance](.agent/skills/sb-repository-maintenance/SKILL.md) for setup, instructions, dependencies, and shared snapshot updates.

Review applicable nested `AGENTS.md` files and [.github/instructions](.github/instructions/) for path-specific context, whether or not the agent loads them automatically. Prompts in [.github/prompts](.github/prompts/) are optional task entry points.

## Repository-specific context

Starbender.Mazer provides .NET 10 ABP theme packages under `src/` and integration demos under `demo/`. [DEVELOPING.md](DEVELOPING.md) documents package boundaries, central dependency files, demo setup, and validation limits.

The library-owned [consumer catalog](docs/consumers/index.md) maps packages and capabilities to integration recipes. Keep it and the consumer plugin current with relevant API changes; [maintenance](docs/consumers/maintenance.md) documents recipe compilation, inventory validation, and portable packaging.

Follow the existing [contribution and release process](.github/CONTRIBUTING.md), [security policy](.github/SECURITY.md), and [CODEOWNERS](.github/CODEOWNERS). Preserve the public theme/module contracts and compatibility across MVC, Blazor Server, WebAssembly, and WebApp when changing shared components.

This repository owns its adapters and [.agent/manifest.yaml](.agent/manifest.yaml). Keep synchronized guidance unchanged locally; propose shared changes upstream and receive updates through reviewable pull requests.
