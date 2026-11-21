# DNX.Extensions — Codebase Recommendations

Analysis of the source, tests, docs, and packaging. CI workflow action versions and dependabot configuration were already addressed separately and are not covered here.

## High impact

### Inconsistent / missing null-guarding
Three different idioms coexist — manual `if (x == null) throw`, a `Guard.IsNotNull(() => x)` helper, and no guard at all — sometimes in the same method.

- `src/DNX.Extensions/Dictionaries/DictionaryExtensions.cs:26-38` mixes `Guard.IsNotNull` with a manual check in one method.
- `src/DNX.Extensions/Arrays/ArrayExtensions.cs:19-33,71-87` (`PadLeft`, `Reduce`) have **no** null check, unlike their sibling `ShiftLeft`/`ShiftRight` in the same file — and the test file confirms the gap (no null-input test for those two).
- The modern `ArgumentNullException.ThrowIfNull` idiom is used nowhere in `src/`.

Worth picking one pattern and sweeping the codebase.

### Silent, undiagnosable exception swallowing
- `src/DNX.Extensions/Objects/ObjectExtensions.cs:33-43,74-84` and `src/DNX.Extensions/DateTimes/DateTimeExtensions.cs:61-71,82-92` do bare `catch { return defaultValue; }` with zero hook to observe what failed.
- Contrast with `src/DNX.Extensions/Execution/RunSafely.cs:16-26` elsewhere in the same codebase, which exposes an `exceptionHandler` callback — that's the better pattern already present and just not applied consistently.

### README doesn't document the API
It's 19 lines — a pitch, a badge, and a note about code-gen — with no mention of the ~20 feature areas (Conversion, Validation, Reflection, Threading, etc.) across 126+ source files. No CHANGELOG exists either. For a public NuGet package this is probably costing adoption.

## Medium impact

- `Guard.cs` (`src/DNX.Extensions/Validation/BuiltInTypes/Guard.cs:18-21`) compiles an `Expression<Func<T>>` on every call just to get a parameter name — expensive for a guard-clause primitive, and only 11 call sites in the whole repo actually use it.
- `src/DNX.Extensions/Arrays/ArrayExtensions.cs:5` has `#pragma warning disable 1591` and ships with zero XML docs on its public methods — the only file in `src/` that does this, invisible to CI because `CS1591` is already suppressed project-wide.
- The `BuildData.ps1` → `.template` → `*.generated.cs` test-data pipeline (under `tests/DNX.Extensions.Tests/Conversion/BuiltInTypes/`) generates files that each carry **every** type's dead `#if`/`#elif` branches, not just their own — adding a type means touching a shared template and N generated files. A `[MemberData]`-per-type approach would scale better.
- SourceLink is configured but no `.snupkg` is produced (no `IncludeSymbols`), and the README is packed into the nupkg but `PackageReadmeFile` isn't set — so it won't render on nuget.org.

## Low impact

- `MutexManager` (`src/DNX.Extensions/Threading/Mutexes/MutexManager.cs`) has no dedicated test file; its static constructor also has a dead lock+null-check (the CLR already guarantees single-run).
- Copyright says "2016-2019" and `PackageReleaseNotes` still says "Migration from DNX.Helpers" (`src/DNX.Extensions/DNX.Extensions.csproj:22,34-36`) — both clearly stale.
- Library targets only `netstandard2.0` with `Nullable` disabled, despite `LangVersion=Latest` and an open "Multi Targeting" item in `To Do.md`.
- `BuildAll.ps1` has unexplained fixed `Start-Sleep` calls and bare-string `throw`s.

## Priority Summary

| Priority | Finding | Location |
|---|---|---|
| High | Silent broad `catch` swallowing with no diagnostics hook | `Objects/ObjectExtensions.cs:33-43,74-84`, `DateTimes/DateTimeExtensions.cs:61-71,82-92` |
| High | Missing null guards on `PadLeft`/`Reduce`, inconsistent with sibling methods | `Arrays/ArrayExtensions.cs:19-33,71-87` |
| High | Three different null-guard idioms in active use, no modern `ArgumentNullException.ThrowIfNull` anywhere | `Dictionaries/DictionaryExtensions.cs:26-38`, repo-wide |
| High | README doesn't document the API surface; no CHANGELOG | `README.md` |
| Medium | `Guard.cs` uses expensive expression-tree compilation for a "cheap guard clause" abstraction, used inconsistently | `Validation/BuiltInTypes/Guard.cs:18-21,81-87` |
| Medium | `ArrayExtensions.cs` undocumented public API, masked by a local `#pragma warning disable 1591` | `Arrays/ArrayExtensions.cs:5` |
| Medium | Template-driven `TestsDataSource.generated.cs` files carry dead `#if`/`#elif` branches per type — hard to extend/review | `tests/DNX.Extensions.Tests/Conversion/BuiltInTypes/TestsDataSource/*.generated.cs` |
| Medium | No symbols package (`.snupkg`) despite SourceLink being configured | `src/DNX.Extensions/DNX.Extensions.csproj` |
| Medium | `PackageReadmeFile` not set — packed README is inert on nuget.org | `src/DNX.Extensions/DNX.Extensions.csproj:40-43` |
| Low | `MutexManager` has no dedicated test file | `src/DNX.Extensions/Threading/Mutexes/MutexManager.cs` |
| Low | Dead lock/null-check in `MutexManager` static constructor | `MutexManager.cs:24-33` |
| Low | Stale copyright year (2019) and stale `PackageReleaseNotes` | `DNX.Extensions.csproj:22,34-36` |
| Low | Single-target `netstandard2.0`, `Nullable` disabled, despite `LangVersion=Latest` and an open "Multi Targeting" TODO | `DNX.Extensions.csproj:5,7`; `To Do.md:7-11` |
| Low | `BuildAll.ps1` fixed sleeps and bare-string throws | `BuildAll.ps1:2,70,84,88,99,111` |
