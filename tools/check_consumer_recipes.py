#!/usr/bin/env python3
"""Compile fenced consumer examples and emit a source-assembly type inventory."""

from __future__ import annotations

import json
from pathlib import Path
import re
import subprocess
import tempfile


ROOT = Path(__file__).resolve().parents[1]
MARKER = re.compile(r"<!-- recipe-check: ([A-Za-z][A-Za-z0-9-]*\.(cs|razor|json)) -->\n```(csharp|razor|json)\n(.*?)\n```", re.DOTALL)
LANGUAGES = {"cs": "csharp", "razor": "razor", "json": "json"}


def main() -> None:
    examples: dict[str, str] = {}
    for page in sorted((ROOT / "docs/consumers").rglob("*.md")):
        content = page.read_text(encoding="utf-8")
        matches = list(MARKER.finditer(content))
        if content.count("<!-- recipe-check:") != len(matches):
            raise ValueError(f"Malformed recipe-check marker in {page.relative_to(ROOT)}")
        for match in matches:
            name, extension, language, code = match.groups()
            if name.casefold() in {item.casefold() for item in examples}:
                raise ValueError(f"Duplicate recipe filename: {name}")
            if language != LANGUAGES[extension]:
                raise ValueError(f"Recipe filename/language mismatch: {name}")
            if extension == "json":
                json.loads(code)
            examples[name] = code + "\n"

    if not examples:
        raise ValueError("No checked consumer examples found")

    output = ROOT / "artifacts/consumer"
    output.mkdir(parents=True, exist_ok=True)
    # Every run compiles only the current documentation, with no stale snippets.
    with tempfile.TemporaryDirectory(prefix="recipe-check-", dir=output) as temporary:
        directory = Path(temporary)
        for name, code in examples.items():
            (directory / name).write_text(code, encoding="utf-8", newline="\n")
        subprocess.run(
            [
                "dotnet", "run", "--project", str(ROOT / "tools/ConsumerRecipes/ConsumerRecipes.csproj"),
                "--configuration", "Debug", f"-p:ConsumerRecipeDirectory={directory}",
                "--", str(output / "inventory.json"), str(directory / "theme-settings.json"),
            ], cwd=ROOT, check=True,
        )
    print(f"Compiled {sum(name.endswith(('.cs', '.razor')) for name in examples)} recipe snippets; "
          "checked theme configuration/serialization and explicit selection; "
          "emitted artifacts/consumer/inventory.json")


if __name__ == "__main__":
    main()
