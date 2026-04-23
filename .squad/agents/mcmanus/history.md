# Project Context

- **Owner:** rido-min
- **Project:** FluentCards — multi-language library for building Adaptive Cards using fluent builder patterns with strong typing and schema validation
- **Stack:** C#/.NET 8, TypeScript/Node.js, Python 3.10+, Go 1.22+ (prototyping)
- **Port:** dotnet/ (C#/.NET 8)
- **Schema:** Adaptive Cards 1.6.0
- **Created:** 2026-04-15

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### 2025-07 — Codebase Review Findings

- **Build/test baseline**: 0 warnings, 580 tests pass, 3 benchmark tests. Command: `cd dotnet && dotnet build --configuration Release && dotnet test --configuration Release --no-build`.
- **Project layout**: Library `dotnet/src/FluentCards/`, tests `dotnet/tests/FluentCards.Tests/`, samples `dotnet/samples/FluentCards.Samples/` + `dotnet/samples/TeamsCardRenderer/`, benchmarks `dotnet/benchmarks/`.
- **JSON serialization**: `FluentCardsJsonContext.cs` is the source-generated context (camelCase, ignore nulls, indented). Custom converters: `CamelCaseEnumConverter<T>`, `FallbackConverter`, `InlinesConverter`, `TargetElementListConverter`, `ActionDataConverter`.
- **Polymorphism**: `[JsonPolymorphic]` + `[JsonDerivedType]` on `AdaptiveElement` (16 types) and `AdaptiveAction` (5 types).
- **Known issues**: (1) `Serialization/` folder uses `FluentCards.Serialization` namespace — violates no sub-namespaces rule. (2) `InputElement.Id` hides base `AdaptiveElement.Id` with `new`. (3) `Column` doesn't extend `AdaptiveElement`, duplicates all base properties. (4) Dual validation systems (`AdaptiveCardExtensions.Validate` vs `AdaptiveCardValidator.Validate`). (5) `AdaptiveCardSerializer.Serialize(indented:false)` allocates new `JsonSerializerOptions` per call.
- **Enums**: All use `CamelCaseEnumConverter<T>` extending `JsonStringEnumConverter<T>`. Complete for AC 1.6.0.
- **All public types have XML doc comments** — `GenerateDocumentationFile=true` in csproj, zero doc warnings.
- **File-scoped namespaces** used everywhere.
- **`TeamsAdaptiveCards` and `TeamsAdaptiveCardInputs`** are .NET-specific convenience types not in the cross-port sample parity list.

### 2026-04-15: Coordinated Full-Team Codebase Review

Led by Keaton with deep-dives from Fenster (TS), McManus (.NET), Hockney (Python), Verbal (tests). McManus audited .NET specifically and found: TextBlock missing selectAction property and builder methods (CRITICAL — spec conformance bug), namespace violation in Serialization/, serializer performance issue (allocates JsonSerializerOptions on every indented:false call), InputElement.Id shadowing, Column inheritance gaps, dual validation systems, weak typing (object?, List<object>), missing [JsonSerializable] registrations. These findings compiled into mcmanus-codebase-review.md. .NET identified as reference port for both feature completeness (583 tests vs 102–63 elsewhere) and coverage quality. TextBlock.selectAction fix prioritized as highest-severity task across all ports.

### 2026 — Issue #75: Native Object Serialization Methods

- Added `SerializeToElement()` and `SerializeToNode()` to `AdaptiveCardSerializer` — no string intermediary for embedding cards in larger payloads.
- Added `ToJsonElement()` and `ToJsonNode()` extension methods on `AdaptiveCard` for ergonomic use.
- Added `WithData<T>()` generic overload to `ActionBuilder` — serializes any type registered in `FluentCardsJsonContext` to `JsonElement` directly.
- All new serialization goes through the source-generated `FluentCardsJsonContext` for AOT compatibility.
- `JsonSerializer.SerializeToElement()` and `SerializeToNode()` are the .NET 8 built-in methods that work with source generators — no custom plumbing needed.
- Test count increased from 698 to 707 (9 new tests in `NativeObjectSerializationTests.cs`).
- Key files: `Serialization/AdaptiveCardSerializer.cs`, `AdaptiveCardExtensions.cs`, `ActionBuilder.cs`.
