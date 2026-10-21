---
applyTo: "**/tests/**,**/Tests/**,**/*Tests.cs,**/*Tests.csproj,**/*Test.cs,**/test_*.py"
---

Read [AGENTS.md](../../AGENTS.md) and the [testing standard](../../.agent/standards/testing.md). Use the [defect workflow](../../.agent/workflows/defect-investigation.md) for regression investigation.

There are currently no automated .NET test projects. Consumer packaging tests use Python's standard-library unittest runner: `py -3.12 tools/test_consumer_tooling.py` (or `python tools/test_consumer_tooling.py` with Python 3.11+). They verify extracted bundle behavior without installing or invoking agent clients. Follow the build and demo validation guidance in [DEVELOPING.md](../../DEVELOPING.md#environment-and-commands), and update that document and these globs when adding another test suite.
