# Progress

Work state for Bennewitz.Ninja.JsonC, updated in the same change as the work it describes. What has stopped
changing moves out rather than piling up.

## Published

| Version | Released | What it is |
|---|---|---|
| `2026.3.926` | 2026-09-25, 23:35 UTC | The first release on nuget.org: the same code as ClaudeForge's `2026.3.925`, published from this repository. What changed for a consumer is in its [release notes](https://github.com/JanusMael/Bennewitz.Ninja.JsonC/releases/tag/v2026.3.926) |

Versions up to `2026.3.925` exist only on ClaudeForge's GitHub Packages feed, from before the move.

Verified against nuget.org, not the workflow: the flat container lists `2026.3.926`, and the
downloaded package's `lib/net10.0/JsonC.dll` carries `PublicVersion` `2026.3.926`, commit `2ea05fd`
and `[AssemblyMetadata("IsTrimmable", "True")]`. Decompiled beside `2026.3.925`'s, it has the same
types, members and method bodies; only the build stamps and `InternalsVisibleTo` differ.

## Drift from ClaudeForge `plans/00007`

- **Released on the day decision 4 ruled out.** The maintainer chose to release `2026.3.926` on
  2026-09-25, knowing the build would stamp the assembly and file version `2026.3.925.2335`, the
  build's UTC time. `2026.3.926` is spent a day early, so the next version is `2026.3.927`, on
  2026-09-27 at the earliest.

## On GitHub and nuget.org

Set up on 2026-09-25. The template is applied and nothing needs generating again: this repository
was made from Templates `2026.3.925`, and no remaining step needs it installed.

| What | State | Verified by |
|---|---|---|
| Repository | [JanusMael/Bennewitz.Ninja.JsonC](https://github.com/JanusMael/Bennewitz.Ninja.JsonC), public | `git ls-remote origin main` showed the local `main` hash |
| Conventions | applied: settings, topics, security toggles, and the `main` and `release-tags` rulesets | `check --admin` conforms; CI's `conventions` job is green |
| `NUGET_USER` | a repository variable, `JanusMael` | `gh variable get NUGET_USER` reads it back |
| Trusted publishing | one policy on nuget.org, created by the maintainer, pattern `Bennewitz.Ninja.JsonC` | the preflight, Release run `36193370799`, and the first release, run `36201599291`, which pushed with it |

`main` takes pull requests only: its ruleset requires `build`, `pack` and `conventions`. Only a
repository admin can create, move or delete a `v*` tag.

## The import

The initial import (ClaudeForge `plans/00007`, Phase A): generated from Templates `2026.3.925` with
`dotnet new bbpkg -n JsonC --RepoOwner JanusMael`, the template's sample replaced by JsonC's source
and tests from ClaudeForge `46fe13d`. Verified before the first commit:

| Check | Result |
|---|---|
| Moved files byte-identical to `46fe13d` (git blob hash of the normalised content) | 19 of 19 |
| Test names | ClaudeForge's 73 plus the template's 7 packaging tests, none missing (TRX name sets, not counts) |
| Build, Release | 0 warnings |
| `TrimmableTests` can fail | reddened with `-p:IsTrimmable=false`, green again without it |
| `check --offline` | clean, including the build properties with `"trimming": "required"` |
| Pack | one package, `Bennewitz.Ninja.JsonC`; `assert-packages` agrees with `packages.push` |

Beyond the template, the import needed three things the moved tests and source depend on:

- `tests/Shared/MessageAssert.cs`, the MSTest-to-xUnit converter's helpers, linked by
  `tests/Directory.Build.props` with a global `using Bennewitz.Ninja.Testing`.
- `NoWarn CS1591` in `src/JsonC/JsonC.csproj`: some enum members have no doc comment, and the
  source moved unchanged. ClaudeForge's project suppressed it too.
- `src/JsonC/AssemblyInfo.cs`, granting `JsonC.Tests` the internal `JsoncEditor.Quote`.

## Resume here

Phase A of ClaudeForge `plans/00007` is done. On 2026-09-25 the ClaudeForge session was sent a message
saying `2026.3.926` is live, with what its Phase B needs to know; consuming the package and removing
JsonC from ClaudeForge is that repository's work. Nothing is pending here. A change to JsonC now
ships in a version of its own, `2026.3.927` at the earliest. The release runbook is
[`docs/publishing.md`](docs/publishing.md).
