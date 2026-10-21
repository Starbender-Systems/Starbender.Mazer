"""Exercise the extracted multi-host bundle without installing into user agents."""
from __future__ import annotations

import contextlib
import hashlib
import io
import json
from pathlib import Path
import tempfile
import unittest
import zipfile

import consumer_tooling as tooling


ROOT = Path(__file__).resolve().parents[1]


class ConsumerBundleTests(unittest.TestCase):
    def test_extracted_bundle_serves_all_hosts_and_repository_skills(self):
        source, catalog, plugin = tooling.validate(ROOT)
        name = catalog["plugin"]
        plugin_root = f"plugins/{name}"
        skill_root = f"{plugin_root}/skills/{name}"
        # Exercise the optional API inventory path as well as the default bundle.
        inventory = tooling.json_bytes({
            "schema_version": 1,
            "packages": [{**package, "symbols": sorted({symbol
                for capability in catalog["capabilities"]
                if package["id"] in capability["packages"]
                for symbol in capability["symbols"]})} for package in catalog["packages"]],
        })
        with tempfile.TemporaryDirectory(prefix="mazer-host-bundle-") as directory:
            temporary = Path(directory)
            inventory_path = temporary / "inventory.json"
            inventory_path.write_bytes(inventory)
            for with_inventory in (False, True):
                with self.subTest(inventory=with_inventory):
                    output = temporary / ("with-inventory" if with_inventory else "without-inventory")
                    with contextlib.redirect_stdout(io.StringIO()):
                        tooling.bundle(ROOT, output, inventory_path if with_inventory else None, preview=True)
                    archive_path, = output.glob("*.zip")
                    with zipfile.ZipFile(archive_path) as archive:
                        paths = archive.namelist()
                        self.assertEqual(len(paths), len(set(paths)))
                        self.assertTrue(all(not p.startswith("/") and ".." not in Path(p).parts
                                            and "\\" not in p for p in paths))
                        extracted = output / "unrelated" / "marketplace"
                        archive.extractall(extracted)
                    files = {p.relative_to(extracted).as_posix(): p.read_bytes()
                             for p in extracted.rglob("*") if p.is_file()}
                    tooling.check_links(files)

                    # Existing OpenAI identity, policies, and interface are preserved.
                    openai = json.loads(files[tooling.MARKETPLACE])
                    authored = json.loads(source[tooling.MARKETPLACE])
                    self.assertEqual(openai, {**authored, "plugins": [p for p in authored["plugins"]
                                                                   if p["name"] == name]})
                    self.assertEqual(files[f"{plugin_root}/plugin.json"], source[f"{plugin_root}/plugin.json"])

                    # Both new hosts resolve the same plugin and default skills directory.
                    marketplace = json.loads(files[tooling.CLAUDE_MARKETPLACE])
                    self.assertEqual(marketplace, {
                        "name": openai["name"], "owner": {"name": plugin["author"]["name"]},
                        "plugins": [{"name": name, "source": f"./{plugin_root}"}],
                    })
                    claude = json.loads(files[f"{plugin_root}/.claude-plugin/plugin.json"])
                    for key in ("name", "version", "description", "repository", "author"):
                        self.assertEqual(claude[key], plugin[key])
                    self.assertNotIn("extensions", claude)
                    self.assertEqual([p for p in files if p.endswith("/SKILL.md")], [f"{skill_root}/SKILL.md"])
                    self.assertEqual(files[f"{skill_root}/SKILL.md"], source[f"{skill_root}/SKILL.md"])
                    for path, data in source.items():
                        if path.startswith("docs/consumers/"):
                            self.assertEqual(files[f"{skill_root}/references/{path.removeprefix('docs/consumers/')}"], data)

                    provenance = json.loads((output / "provenance.json").read_bytes())
                    self.assertTrue(provenance["preview"])
                    self.assertEqual(provenance["api_inventory_included"], with_inventory)
                    self.assertEqual(provenance["plugin"], {"name": name, "version": plugin["version"]})
                    self.assertEqual(files[f"{skill_root}/references/provenance.json"],
                                     (output / "provenance.json").read_bytes())
                    expected = set(files) - {f"{skill_root}/references/provenance.json"}
                    self.assertEqual(set(provenance["files"]), expected)
                    for path, digest in provenance["files"].items():
                        self.assertEqual(hashlib.sha256(files[path]).hexdigest(), digest)
                    inventory_target = f"{skill_root}/references/api/inventory.json"
                    self.assertEqual(inventory_target in files, with_inventory)
                    if with_inventory:
                        self.assertEqual(files[inventory_target], inventory)
                    for line in (output / "SHA256SUMS").read_text().splitlines():
                        digest, filename = line.split("  ", 1)
                        self.assertEqual(hashlib.sha256((output / filename).read_bytes()).hexdigest(), digest)

                    # A copied repository skill stays closed under its own directory.
                    # No plugin root, marketplace, or original checkout is needed for links.
                    copied = {f".github/skills/{name}/" + p.removeprefix(skill_root + "/"): data
                              for p, data in files.items() if p.startswith(skill_root + "/")}
                    tooling.check_links(copied)
                    self.assertIn(f".github/skills/{name}/references/index.md", copied)

                    repeated = temporary / (output.name + "-repeat")
                    with contextlib.redirect_stdout(io.StringIO()):
                        tooling.bundle(ROOT, repeated, inventory_path if with_inventory else None, preview=True)
                    self.assertEqual(archive_path.read_bytes(), (repeated / archive_path.name).read_bytes())

    def test_rejects_missing_owner_and_authored_adapter_drift(self):
        files = tooling.source_files(ROOT)
        catalog, plugin = tooling.validate_files(files)
        plugin_path = f"plugins/{catalog['plugin']}/plugin.json"
        for author in (None, {}, {"name": ""}):
            with self.subTest(author=author):
                invalid = {**files, plugin_path: tooling.json_bytes({**plugin, "author": author})}
                with self.assertRaises(tooling.ToolingError):
                    tooling.validate_files(invalid)
        adapter = f"plugins/{catalog['plugin']}/.claude-plugin/plugin.json"
        with self.assertRaisesRegex(tooling.ToolingError, "generated from plugin.json"):
            tooling.validate_files({**files, adapter: tooling.json_bytes(plugin)})

    def test_rejects_broken_links_after_relocation(self):
        files = tooling.source_files(ROOT)
        with self.assertRaisesRegex(tooling.ToolingError, "broken or nonportable local link"):
            tooling.validate_files({**files, "docs/consumers/agent-setup.md": b"[escape](../../DEVELOPING.md)\n"})


if __name__ == "__main__":
    unittest.main()
