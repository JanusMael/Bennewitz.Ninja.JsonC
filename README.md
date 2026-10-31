# Bennewitz.Ninja.JsonC

A comment- and formatting-preserving JSONC (JSON with comments) reader and an edit-based writer for
.NET. It edits a config file by replacing the smallest span of text each change needs rather than
re-serializing the document, so comments, blank lines, key order, indentation and line endings
survive a save untouched. No package dependencies, and trimmable.

## Install

```bash
dotnet add package Bennewitz.Ninja.JsonC
```

## Licence

MIT. See [LICENSE](https://github.com/JanusMael/Bennewitz.Ninja.JsonC/blob/main/LICENSE).
