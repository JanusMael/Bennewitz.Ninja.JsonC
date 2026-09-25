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

## Next

1. Create `JanusMael/Bennewitz.Ninja.JsonC` and push `main`; `apply`, then `check --admin`.
2. A nuget.org trusted-publishing policy for `release.yml`, per `docs/publishing.md`.
3. The first release on a day after `2026.3.925`, verified against nuget.org itself.
4. ClaudeForge then consumes it (its `plans/00007`, Phase B).
