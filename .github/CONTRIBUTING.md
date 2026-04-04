# Contributing to Starbender.Mazer

Thanks for your interest in contributing to Starbender.Mazer.

This repository uses a fork-and-pull-request contribution model.

## Workflow

1. Fork the repository to your own GitHub account.
2. Create a topic branch from `development` for your change.
3. Make your changes and add or update tests when appropriate.
4. Open a pull request back to `development`.
5. Wait for CI to pass and for maintainer review before merge.

Changes intended for release should flow through `development` first. Releases are promoted to `main` by maintainers.

## Pull Request Expectations

- Keep pull requests focused on a single change or tightly related set of changes.
- Include a clear summary of what changed and why.
- Link related issues when applicable.
- Update documentation when behavior, setup, or public APIs change.
- Follow the existing code style and project structure.

## Local Development

1. Install the .NET SDK version used by the repository.
2. Restore dependencies with `dotnet restore`.
3. Build the solution with `dotnet build Starbender.Mazer.slnx --configuration Release`.
4. Run tests relevant to your change before opening a pull request.

## Branching and Releases

- `development` is the integration branch.
- `main` is the stable release branch.
- Pull requests should target `development` unless a maintainer asks otherwise.

Package publishing is handled by GitHub Actions after pull requests are merged:

- Merges into `development` publish prerelease packages.
- Merges into `main` publish release packages.

## Reporting Bugs and Requesting Features

- Use GitHub Issues for bugs, feature requests, and general repository work.
- For security issues, do not open a public issue. Follow the guidance in [`SECURITY.md`](SECURITY.md).

## Code of Conduct

By participating in this project, you agree to follow the [`CODE_OF_CONDUCT.md`](CODE_OF_CONDUCT.md).
