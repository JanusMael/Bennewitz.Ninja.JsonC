# Progress

Work state for Bennewitz.Ninja.JsonC, updated in the same change as the work it describes. What has stopped
changing moves out rather than piling up.

## Published

| Version | Released | What it is |
|---|---|---|
| `2026.3.926` | 2026-09-25, 23:35 UTC | The first release on nuget.org: the same code as ClaudeForge's `2026.3.925`, published from this repository. What changed for a consumer is in its [release notes](https://github.com/JanusMael/Bennewitz.Ninja.JsonC/releases/tag/v2026.3.926) |
| `2026.4.1031` | 2026-10-31, 18:57 UTC | The sync to Templates `2026.4.1031`, below. The flat container lists it, and the GitHub release carries the nupkg |

Versions up to `2026.3.925` exist only on ClaudeForge's GitHub Packages feed, from before the move.

Verified against nuget.org, not the workflow: the flat container lists `2026.3.926`, and the
downloaded package's `lib/net10.0/JsonC.dll` carries `PublicVersion` `2026.3.926`, commit `2ea05fd`
and `[AssemblyMetadata("IsTrimmable", "True")]`. Decompiled beside `2026.3.925`'s, it has the same
types, members and method bodies; only the build stamps and `InternalsVisibleTo` differ.

## Drift from ClaudeForge `plans/00007`

- **Released on the day decision 4 ruled out.** The maintainer chose to release `2026.3.926` on
  2026-09-25, knowing the build would stamp the assembly and file version `2026.3.925.2335`, the
  build's UTC time. `2026.3.926` was spent a day early.

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

## Synced to Templates `2026.4.1031`

On 2026-10-31, closing #4 and #6:

- `scripts/repo-conventions.cs` and `scripts/assert-packages.cs` are that release's.
- The retired `"trimming"` key is gone from `repository.json`. The repository still trims through
  its projects: `src/Directory.Build.props` sets `IsTrimmable` and `EnableTrimAnalyzer`.
- The family's friend grants: `AssemblyInfo.InternalsVisibleTo.cs`, written by `grants`, and an
  empty `.External.cs`, linked into every project by a root `Directory.Build.targets`, the
  template's. `src/JsonC/AssemblyInfo.cs` and its grant to `JsonC.Tests` are gone: the
  solution-wide file grants the same.
- `PublicVersion` moved from `Directory.Build.props` to that `Directory.Build.targets`, as the
  template's is. In the props file `Version` has no value yet, so a local build recorded an empty
  `PublicVersion`; a release, which passes `-p:Version=`, was not affected.
- The packed README drops its *Releasing* section, as the template's has, and links the licence
  absolutely, which `assert-packages` now requires: nuget.org resolves a relative link against
  itself. The release runbook stays in `docs/publishing.md`, which `AGENTS.md` names.

Build with no warnings, 80 of 80 tests, pack, `assert-packages`, and `check`, which conforms, as CI
runs them. Released as `2026.4.1031` the same day, on the maintainer's word: tag `v2026.4.1031` on
`48b7ccf`, release run `41231823839`, one package pushed.

Then, in the tests alone, Templates' xunit part 2: xunit.v3 `4.0.2`, and TrxReport gone, which
nothing asked a `.trx` of. `Parallelization.cs`'s `CollectionPerClass` still compiles on 4.x; only
`DisableTestParallelization` became an error. 80 of 80 tests. The package does not change.

## Synced to Templates `2026.4.1103`

On 2026-11-03:

- `scripts/repo-conventions.cs` is that release's, which also checks that markdown is checked out
  as LF. The root `.gitattributes` already does that.
- `Microsoft.VisualStudio.Threading.Analyzers` `18.12.1`, referenced privately from
  `Directory.Build.props`, with only `VSTHRD101` and `VSTHRD110` on: the family's decision of
  2026-11-03. Neither reports anything here.

Build with warnings as errors, 80 of 80 tests, pack, `assert-packages`, and `check`, which conforms,
as CI runs them. The packed nuspec gains no dependency. The package does not change.

## Synced to Templates `2026.4.1109`

On 2026-11-09. The scripts were already that release's; it moves the pins:

- AutoVersioning `2026.3.916` to `2026.4.1031`, whose diagnostics are `BNAV00` to `BNAV04` from
  `2026.3.928` on.
- xunit.v3 `4.0.2` to `4.0.3`, whose analyzers report a collection definition nothing joins
  (xUnit1073). `tests/Shared/MessageAssert.cs` held the converter's `DoNotParallelize` definition,
  which no class here joins, so it is gone; Templates' converter now emits it suppressed.

Build with warnings as errors, 80 of 80 tests, pack, `assert-packages`, and `check`, which conforms,
as CI runs them. The package does not change.

## Inspections' AssemblyQuality checks

On 2026-11-10. `bbpkg` gained `tests/<Stem>.Tests/Architecture/AssemblyQualityTests.cs` after this
repository was generated, and the syncs above moved pins only, so it never arrived. Now it has:
`Bennewitz.Ninja.AssemblyQuality` `2026.4.1031`, referenced by `JsonC.Tests`, runs BNAQ1001,
BNAQ1002, BNAQ1004 and BNAQ1005 over the compiled `JsonC.dll`.

- BNAQ1002's default set names `System.Text.Json.Nodes`, and `JsoncEditor.SetValue` and
  `SetValueEdits` take a `JsonNode` by design. By the maintainer's decision of 2026-11-10 the test
  pins those two findings, so any other DOM type in the public surface fails it. `AGENTS.md`
  records the surface as an invariant.
- Each rule that can fail here was made to: a canary adding a public method that returns a
  `JsonObject` and a grant to an undeclared assembly reddened BNAQ1002 and BNAQ1005, and the suite
  passed again without it.
- `src/Directory.Build.props` now names `TrimmableTests`, which reads the trimmable mark, where it
  named a `PackageMetadataTests` this repository never had. `NuGet.config` gains the template's
  warning about a nested config's mapping.

Build with warnings as errors, 84 of 84 tests, pack, `assert-packages`, and `check --offline`, which
conforms. The package does not change.

## The template's release, through `release-target.cs`

On 2026-11-10, the other half of what Inspections brought to `bbpkg`. `release.yml` now resolves
every release through `scripts/release-target.cs` before anything is built, which refuses a tag of
the wrong kind and a version that is not `YYYY.Q.MMDD` with its date's quarter. JsonC declares no
families, so a release is what it was: a `v<version>` tag, every id in `packages.push`, GitHub's
generated notes.

- `release.yml`, `scripts/release-target.cs`, `docs/publishing.md`, `scripts/AGENTS.md` and
  `PackagingTests` are Templates `2026.4.1109`'s, tokens filled in. `PackagingTests` now checks that
  the workflow resolves through the script, and counts a project packable unless it says otherwise,
  where it counted only an explicit `IsPackable`. With the old workflow restored it fails.
- `AGENTS.md` takes the template's rows for the release, the packed README's links and the friend
  grants, and its `grants` command.
- `release-target.cs resolve`, run locally: `v2026.4.1110` resolves to `2026.4.1110` and
  `Bennewitz.Ninja.JsonC`; `v2026.3.1110` is refused for its quarter, `json/v2026.4.1110` as a family
  tag where none is declared, and `v2026.4.1110.1` for having four parts.
- The blank-version preflight, dispatched on the branch before the merge: Release run `42905820625`
  at `cd6a019` passed, the token exchange succeeded and `check --release` conforms.

Build with warnings as errors, 84 of 84 tests, pack, `assert-packages`, and `check --offline`, which
conforms. The package does not change.

## Synced to Templates `2026.4.1111`

On 2026-11-11, on the maintainer's word, from the Templates session.

- `scripts/repo-conventions.cs` is that release's. It also fails two things:
  - a repository whose `packages.push` names an id but whose release does not resolve through
    `scripts/release-target.cs`;
  - a repository that does not pin `Bennewitz.Ninja.AssemblyQuality`.

  This repository meets both, since #11 and #12.
- `Bennewitz.Ninja.CodeQuality` `2026.4.1104` is referenced privately, every rule at its default, the
  family's decision of 2026-11-10. It reported two `BNCQ1002` findings, `JsoncEditor.SetValue` and
  `SetValueEdits` taking a `JsonNode`: the same two that `AssemblyQualityTests` pins for BNAQ1002,
  by design. Each is suppressed on its method with that reason, and any other DOM type in the public
  surface still fails the build.

Build with warnings as errors, 84 of 84 tests, pack, `assert-packages`, and `check`, which conforms,
as CI runs them. The package does not change.

## Resume here

Nothing is pending here. ClaudeForge consumes JsonC from nuget.org alone, pinned at `2026.4.1031`
(its `JsonCVersion`, read on 2026-11-10). A public API change is told to ClaudeForge before it
ships, per `AGENTS.md`.

A change to JsonC ships in a version of its own. The release runbook is
[`docs/publishing.md`](docs/publishing.md).
