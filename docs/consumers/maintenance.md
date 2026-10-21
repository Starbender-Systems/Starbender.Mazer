# Maintain consumer tooling

The library owns this catalog, its plugin, and `tools/consumer_tooling.py`. The generic tool was copied at bootstrap and is not updated by engineering-standards snapshot synchronization. Review later tooling/template changes explicitly. Canonical guides live under `docs/consumers`; the bundler assembles them under the packaged skill's `references/` directory.

Update capability routes, package versions, source evidence, and affected recipes when reusable behavior changes. Increment the consumer plugin's version in `plugin.json` when bundled guidance changes; its version is independent of library and engineering-standards versions. Evaluate package metadata rather than inferring IDs or versions from filenames. Confirm feed availability separately from source declarations.

Run from the repository root with Python 3.11+ and the .NET 10 SDK (on Windows, `py -3.12` can replace `python`):

```text
python tools/check_consumer_recipes.py
python tools/consumer_tooling.py validate . --inventory artifacts/consumer/inventory.json
python tools/consumer_tooling.py bundle . --output ../consumer-tooling-preview --allow-dirty-preview --inventory artifacts/consumer/inventory.json
```

The recipe checker scans all consumer guides (including [installation](installation.md)), extracts fenced blocks marked `recipe-check` into a fresh temporary directory under ignored `artifacts/consumer/`, and compiles their C# and Razor against all seven source projects. It exercises hierarchical and scalar-JSON registration, default values for absent/empty theme settings, rejection of invalid/null JSON, serializer round trips, and the documented MVC/Blazor selection adapters overriding existing default names. This checks options registration, not a complete ABP application startup. `tools/ConsumerRecipes` is a validation executable, not a published package or a replacement for the demos. It emits `artifacts/consumer/inventory.json` with assembly versions and exported **type IDs**. The generic validator checks each capability's package ownership and type references against that inventory. It does not enumerate method signatures or prove binary compatibility. Update the checker's source-project/module list when packages change.

CI checks recipe compilation, inventory-backed catalog validation, portable links, and preview assembly. Browser rendering, installed-package compatibility, deployed assets, ABP installer execution, identity flows, and both InteractiveAuto execution paths still require the acceptance checks in each recipe and a configured consuming application. Do not report those checks as passing from compilation alone.

Extract the ZIP into an unrelated directory and inspect the skill and links there. The archive contains `.agents/plugins/marketplace.json` and `plugins/starbender-mazer-consumer/` with all consumer references and, when supplied, `references/api/inventory.json`. It needs no original library checkout. Generating an archive does not install or enable a plugin.

For a release bundle, commit the tooling and guides, regenerate inventory from that source, then bundle from a clean checkout into a new directory outside the repository:

```text
python tools/consumer_tooling.py bundle . --output ../consumer-tooling-release --inventory artifacts/consumer/inventory.json
```

`library.ref: HEAD` resolves to the actual bundled commit. A release tag/commit may instead be selected when preparing its matching clean checkout; the ref must resolve to that checkout's HEAD. Output includes a deterministic marketplace ZIP, `provenance.json`, and `SHA256SUMS`. Provenance records the source commit, resolved ref, package versions, and bundled file hashes. Preview output is marked in its filename and metadata. Publication follows the library's existing release process; the tool does not publish Git refs, upload releases, or alter user plugin configuration.

Recover by reverting the library-owned tooling/guidance together and rebuilding from the recorded revision. Never replace a published immutable plugin version with different bytes.
