# AGENTS.md

## Repo Structure

FluentCards is a multi-language library. Each language port lives in its own top-level folder:

| Folder | Language | Status |
|--------|----------|--------|
| `dotnet/` | C# / .NET 8 | Stable |
| `node/` | TypeScript / Node.js | Stable |

Shared assets (docs, screenshots, root README) live at the repository root.

---

## Adding a New Language Port

When implementing a port for a new language, follow these steps:

1. **Create a top-level folder** named after the language ecosystem (e.g., `python/`, `java/`, `rust/`).
2. **Mirror the full Adaptive Cards 1.6.0 specification** — all elements, properties, actions, inputs, and enums must conform to `https://adaptivecards.io/schemas/1.6.0/adaptive-card.json`. Do not invent properties not in the spec.
3. **Implement the same core modules** as existing ports:
   - Models / types for all card elements
   - Enums for all string-enumerated properties
   - Fluent builder classes: `Create() → withX() / addX() → build()`
   - Serialization: `toJson()` / `fromJson()`
   - Validation: `validate()` and `validateAndThrow()`
4. **Add a test suite** covering builders, serialization, and validation. Aim for parity with existing ports.
5. **Add a `{lang}/README.md`** with a quick-start example and a link to the root README.
6. **Add CI job(s)** to `.github/workflows/ci.yml` using `defaults.run.working-directory: {lang}`.
7. **Add a language-specific section** to this `AGENTS.md`.
8. **Update the root `README.md`** to include the new port in the Language Ports table.

### Definition of Done (all ports)
- All tests pass.
- No new build warnings.
- Changes scoped to the request — no drive-by refactors.

---

## dotnet/

### Verification
```
cd dotnet
dotnet build --configuration Release && dotnet test --configuration Release --no-build
```
If it fails, fix the root cause and re-run before committing.

### Environment
- .NET 8.0 with `LangVersion=latest` and `Nullable=enable`.
- Use file-scoped namespaces (`namespace FluentCards;`).
- All library code lives in the `FluentCards` namespace — do not add sub-namespaces.

### Guardrails
- This library implements the **Adaptive Cards 1.6.0 specification** (`https://adaptivecards.io/schemas/1.6.0/adaptive-card.json`). All elements, properties, actions, and enums must conform to that schema.
- AOT-compatible (`IsAotCompatible=true`). Do not use reflection-based serialization, `dynamic`, or APIs that break trimming.
- JSON serialization uses System.Text.Json source generators. When adding a new serializable type, register it with `[JsonSerializable]` in `FluentCardsJsonContext.cs`.
- Follow the builder pattern: `Create()` → `WithX()` / `AddX(Action<TBuilder>)` → `Build()`. Each Adaptive Card element gets its own builder class.
- All public types and members must have XML doc comments (`<summary>`).
- Tests use xunit. Test files are in `tests/FluentCards.Tests/` and named `{Feature}Tests.cs`.

### Constraints
- Keep diffs minimal and scoped to the request.
- Update or add tests for any behavior change.
- Do not modify CI, dependency versions, or security settings unless asked.
- Never print, log, or commit secrets.

---

## node/

### Verification
```
cd node
npm install && npm test && npm run typecheck
```

### Environment
- TypeScript with strict mode enabled.
- Node.js built-in test runner (`node:test`) with `tsx` for TypeScript support.
- Library lives in `node/fluent-cards/src/`. Tests live in `node/fluent-cards-tests/test/`.

### Guardrails
- This library implements the **Adaptive Cards 1.6.0 specification**. All elements, properties, actions, and enums must conform to the schema.
- Use TypeScript string enums (e.g., `TextSize.ExtraLarge = 'extraLarge'`) — they serialize correctly without custom converters.
- Use interfaces + discriminated unions instead of class hierarchies; type narrowing via `element.type === 'TextBlock'`.
- Use `undefined` for optional fields — `JSON.stringify` omits them automatically.
- Builder methods return `this` for fluent chaining; `build()` returns a plain model object.

### Constraints
- Keep diffs minimal and scoped to the request.
- Update or add tests for any behavior change.
- Do not modify CI, dependency versions, or security settings unless asked.
- Never print, log, or commit secrets.
