# Bennewitz.Ninja.JsonC

A comment- and formatting-preserving JSONC (JSON with comments) reader and an edit-based writer for
.NET. It edits a config file by replacing the smallest span of text each change needs rather than
re-serializing the document, so comments, blank lines, key order, indentation and line endings
survive a save untouched. No package dependencies, and trimmable.

## Install

```bash
dotnet add package Bennewitz.Ninja.JsonC
```

## Releasing

See [docs/publishing.md](docs/publishing.md). The short version:

1. Add the `NUGET_USER` repository **variable**, not a secret: your nuget.org **profile name**,
   not an email.
2. Create **one** trusted-publishing policy whose glob patterns cover every id in
   [`packages.push`](packages.push) and match nothing in [`packages.local`](packages.local).
3. Run **Release** → *Run workflow* with the version **blank**. That logs in and stops, proving the
   credentials without publishing.
4. Tag `vYYYY.Q.MMDD` and push.

⛔ **One policy, never one per package id.** nuget.org mints one API key per token exchange, scoped
to one matching policy — so a second policy is never consulted and its package is rejected `403`
after the first has already published permanently.

## Licence

MIT. See [LICENSE](LICENSE).
