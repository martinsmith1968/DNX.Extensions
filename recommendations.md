# DNX.Extensions — Codebase Recommendations

Analysis of the source, tests, docs, and packaging. CI workflow action versions and dependabot
configuration were already addressed separately and are not covered here.

This file was last reconciled against the actual codebase on 2026-11-23. Several items from the
original analysis have since been resolved (see "Resolved" below); the open items are what remain.

## Open

### Low impact

- `Guard.cs` (`src/DNX.Extensions/Validation/BuiltInTypes/Guard.cs:18-21`) compiles an `Expression<Func<T>>`
  on every call just to get a parameter name - relatively expensive for a guard-clause primitive.
  This is a deliberate design tradeoff (automatic parameter-name capture vs. raw performance), not
  a bug - worth a conscious decision rather than a reflexive fix.
- `src/DNX.Extensions/Arrays/ArrayExtensions.cs:5` has `#pragma warning disable 1591` and ships
  with zero XML docs on its public methods - the only file in `src/` that does this, invisible to
  CI because `CS1591` is already suppressed project-wide (`DNX.Extensions.csproj:17`).
- The `BuildData.ps1` → `.template` → `*.generated.cs` pipeline (Convert/Maths/Guard built-in-type
  families, plus their test-data counterparts) generates files that each carry **every** type's
  dead `#if`/`#elif` branches, not just their own - adding a type means touching a shared template
  and N generated files. Documented as an accepted, known pattern (see `docs/Conversion.md`,
  `docs/Maths.md`, `docs/Validation.md`), not treated as a bug to fix.
- Library targets `netstandard2.0;net8.0;net10.0` with `Nullable` disabled project-wide, despite
  `LangVersion=Latest`. Several recently-added methods (`StreamExtensions.WriteAllText`,
  `IO.PathExtensions.CombineWith`, `Guard.IsNotNullOrEmpty`, the `Retry` delegates) have
  null-handling behavior that nullable annotations would make self-documenting at the call site.
  A bigger, cross-cutting change - not a quick win.
- `BuildAll.ps1` has unexplained fixed `Start-Sleep` calls and bare-string `throw`s
  (`BuildAll.ps1:70,84,88,99,111`).
- `MutexManager` (`src/DNX.Extensions/Threading/Mutexes/MutexManager.cs`) has no dedicated test
  file of its own - it's exercised via `tests/DNX.Extensions.Tests/Threading/MutexTests.cs`
  instead (`Acquire`, `AcquireNoWait`, `CanAcquire` are all covered there). Cosmetic at most; not
  an actual coverage gap.

## Resolved

- **Inconsistent / missing null-guarding** - `DictionaryExtensions` and `ArrayExtensions.PadLeft`/`Reduce`
  now consistently use `Guard.IsNotNull`/`Guard.IsNotNullOrEmpty`.
- **Silent, undiagnosable exception swallowing** - `ObjectExtensions.To<T>`/`ChangeType<T>` and
  `DateTimeExtensions.ParseDateAsUtc` (both default-value overloads) now delegate to
  `RunSafely.Execute` and accept an optional `exceptionHandler`, matching the pattern already used
  elsewhere in the codebase instead of a bare `catch { return defaultValue; }`.
- **README doesn't document the API** - expanded with a full namespace table, installation
  instructions, and project background; every namespace now links to a `docs/*.md` reference page
  (property/method tables + runnable examples), verified against current source and tests.
- **No symbols package (`.snupkg`)** - `IncludeSymbols`/`SymbolPackageFormat=snupkg` added to
  `DNX.Extensions.csproj`; verified via `dotnet pack` that both a `.nupkg` and `.snupkg` are produced.
- **Stale `PackageReleaseNotes`** ("Migration from DNX.Helpers") - now points to the GitHub
  Releases page instead of embedding static text that goes stale every release.
- **Dead lock/null-check in `MutexManager`/`Mutex` static constructors** - removed; the CLR
  already guarantees a type's static constructor runs exactly once.
- **Stale copyright year (2019)** - updated to the correct range.
- **Single-target `netstandard2.0`** - now multi-targets `netstandard2.0;net8.0;net10.0`.
- **`PackageReadmeFile` not set** - this claim in the original analysis was already inaccurate at
  the time it was written; it is set (`README.md`).
