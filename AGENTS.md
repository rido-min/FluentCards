# AGENTS.md

## Verification
- Run: `dotnet build --configuration Release && dotnet test --configuration Release --no-build`
- If it fails, fix the root cause and re-run before committing.

## Environment
- .NET 8.0 with `LangVersion=latest` and `Nullable=enable`.
- Use file-scoped namespaces (`namespace FluentCards;`).
- All library code lives in the `FluentCards` namespace — do not add sub-namespaces.

## Guardrails
- This library implements the **Adaptive Cards 1.6.0 specification** (`https://adaptivecards.io/schemas/1.6.0/adaptive-card.json`). All elements, properties, actions, and enums must conform to that schema. Do not invent properties or element types that are not in the spec.
- This library is AOT-compatible (`IsAotCompatible=true`). Do not use reflection-based serialization, `dynamic`, or APIs that break trimming.
- JSON serialization uses System.Text.Json source generators. When adding a new serializable type, register it with `[JsonSerializable]` in `FluentCardsJsonContext.cs`.
- Follow the builder pattern: `Create()` → `WithX()` / `AddX(Action<TBuilder>)` → `Build()`. Each Adaptive Card element gets its own builder class.
- All public types and members must have XML doc comments (`<summary>`).
- Tests use xunit. Test files are in `tests/FluentCards.Tests/` and named `{Feature}Tests.cs`.

## Constraints
- Keep diffs minimal and scoped to the request.
- Update or add tests for any behavior change.
- Do not modify CI, dependency versions, or security settings unless asked.
- Never print, log, or commit secrets.

## Definition of Done
- All tests pass (verify command above).
- No new build warnings introduced.
- Changes are scoped to the request — no drive-by refactors.
