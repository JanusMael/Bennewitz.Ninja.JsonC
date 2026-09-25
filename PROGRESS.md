# Progress

Work state for Bennewitz.Ninja.JsonC, updated in the same change as the work it describes. What has stopped
changing moves out rather than piling up.

## Published

Nothing yet on nuget.org. Versions up to `2026.3.925` exist only on ClaudeForge's GitHub Packages
feed, from before the move.

## On `main`, not yet released

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

This checkout exists **only on this machine**: `main` has no remote yet. Everything below is the
rest of ClaudeForge `plans/00007` Phase A. The plan is approved and frozen in
[JanusMael/ClaudeForge](https://github.com/JanusMael/ClaudeForge/blob/main/plans/00007-jsonc-moves-to-its-own-repository.md);
read it before starting, and record any drift here, never in the plan.

The template is already applied and nothing needs generating again. This repository was made from
Templates `2026.3.925`; no step below needs it installed, and it no longer is. The family's rules are
[`docs/repository-conventions.md`](https://github.com/JanusMael/Bennewitz.Ninja.Templates/blob/main/docs/repository-conventions.md)
in Bennewitz.Ninja.Templates. The release runbook is [`docs/publishing.md`](docs/publishing.md).

| # | Step | Who | Done when |
|---|---|---|---|
| 1 | Re-verify locally first: `dotnet build JsonC.slnx -c Release -warnaserror`, `dotnet test --solution JsonC.slnx -c Release`, `dotnet run --file scripts/repo-conventions.cs -- check --offline` | agent | 0 warnings, 80 passed, "conforms" |
| 2 | Create the repository and push: `gh repo create JanusMael/Bennewitz.Ninja.JsonC --public --source . --remote origin --push`. **Public**: its package goes to nuget.org, and the csproj's `RepositoryUrl` points here | agent, **after the maintainer says go** | `git ls-remote origin main` shows the local `main` hash |
| 3 | `dotnet run --file scripts/repo-conventions.cs -- apply`, then `-- check --admin`. The first CI run is red on `conventions` until `apply` has run, which is expected | maintainer's `gh` login | `check --admin` passes; the `conventions` job goes green on a re-run. From here `main` takes pull requests only |
| 4 | `gh variable set NUGET_USER --body <nuget.org profile name> --repo JanusMael/Bennewitz.Ninja.JsonC`: a **variable**, the **profile name**, never an email or an API key | maintainer | `gh variable get NUGET_USER` reads back the name |
| 5 | **One** trusted-publishing policy at nuget.org, exactly as `docs/publishing.md` step 2 lays out; pattern `Bennewitz.Ninja.JsonC` | maintainer | the policy lists this repository and `release.yml`, with no pending warning |
| 6 | Preflight: Actions → **Release** → *Run workflow* with the version **blank** | maintainer or agent | green, and the summary lists `Bennewitz.Ninja.JsonC` |
| 7 | First release: `git tag -a vYYYY.Q.MMDD -m "Bennewitz.Ninja.JsonC YYYY.Q.MMDD"` and push the tag. The date **must be after 2026-09-25**, so `v2026.3.926` at the earliest | maintainer | see step 8 |
| 8 | Verify against **nuget.org**, never the workflow: the flat container `https://api.nuget.org/v3-flatcontainer/bennewitz.ninja.jsonc/index.json` lists the version; the downloaded `.nupkg` holds `lib/net10.0/JsonC.dll` whose informational version carries the same stamp and which carries `[AssemblyMetadata("IsTrimmable", "True")]` | agent | both read off the downloaded package, not inferred |
| 9 | Tell ClaudeForge. Move the version into **Published** above, then tell the session in `C:\c\cl\OpenForge2k` which version is live (a message if one is running, otherwise a comment on the ClaudeForge PR or issue tracking `plans/00007`). That repository does Phase B | agent | ClaudeForge has the version |

⛔ **Nothing may be published at `2026.3.925` or earlier.** That version already exists on
ClaudeForge's GitHub feed with different metadata, and a version number must never mean two
different packages.

⛔ **A published version is permanent, and the date allows one release per day.** Never re-tag to
fix a failed run: fix forward, per `docs/publishing.md`.

⛔ **The move changes no behaviour.** No API or source change to JsonC before the first release. The
point of the first nuget.org version is to be the same code as ClaudeForge's `2026.3.925`, under new
provenance. Improvements come after that release, in their own versions.

⚠ **A 401 at the token exchange: read `NUGET_USER` first** (`gh variable get NUGET_USER`), then
work the rest of the list in `docs/publishing.md`. On 2026-09-21 the Templates release burned six
runs on the policy fields while the fault was the value itself.

⚠ If `git push` asks for credentials or picks the wrong account, push with
`git -c credential.helper= -c "credential.helper=!gh auth git-credential" push …`.
