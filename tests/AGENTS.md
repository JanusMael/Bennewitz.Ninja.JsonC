# AGENTS.md — `tests/`

The test projects. `tests/Directory.Build.props` makes every project here an xUnit v3 test
executable that is never packed.

| Project | Covers beyond the packaging guards |
|---|---|
| `JsonC.Tests` | The scanner, text edits, and the editor's mutation, preservation, quoting and safety behaviour. Pure string in, string out: no filesystem and no shared state |

`Shared/MessageAssert.cs` holds the helpers the MSTest-to-xUnit conversion emitted, linked into every
test project by `tests/Directory.Build.props`. It is generated: regenerate it with the converter's
`emit-helpers` in Bennewitz.Ninja.Templates `scripts/mstest-to-xunit.cs`, never edit it by hand.
⚠ `OrdinalAssert` keeps MSTest's ORDINAL string comparison; xUnit's default is culture-aware, so
replacing a call with `Assert.Equal` can weaken it without any test going red.

## Rules

| Rule | Why | Guarded by |
|---|---|---|
| Tests run on Microsoft.Testing.Platform | `dotnet test` takes `--solution` and rejects VSTest-only switches | `global.json`, `test.runner` |
| `tests/Directory.Build.props` imports the root props explicitly | Without it, every test project silently loses the root's target framework and nullable settings | the import line itself |
| The tests under `Packaging/` stay as strong as they are | They are the only thing standing between a packaging mistake and a permanent release | `PackagingTests`, `TrimmableTests` |

⛔ **Never weaken a packaging test to make it pass.** `PackagingTests` reads the project files and
both package lists; `TrimmableTests` reads the trimmable mark off the compiled assembly. When one
fails, the package list or the project is wrong, not the test.
