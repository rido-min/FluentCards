# AGENTS.md

## Repo Structure

FluentCards is a multi-language library. Each language port lives in its own top-level folder:

| Folder | Language | Status |
|--------|----------|--------|
| `dotnet/` | C# / .NET 8 | Stable |
| `node/` | TypeScript / Node.js | Stable |
| `python/` | Python 3.10+ | Stable |
| `go/` | Go 1.22+ | Stable |

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

## Sample Parity

All four language ports share an identical set of sample programs. **When adding, removing, or changing a sample in one language, apply the equivalent change to all others.** The canonical sample list is:

| Sample file (stem) | What it demonstrates |
|--------------------|----------------------|
| `basic_card_sample` | TextBlock, Image, simple layout |
| `form_card_sample` | Input elements (text, number, date, toggle, choice set) |
| `layout_card_sample` | ColumnSet, Container, FactSet |
| `people_picker_sample` | People picker via `with_choices_data` / dynamic dataset |
| `rich_content_sample` | RichTextBlock, ImageSet, Media, ActionSet |
| `validation_sample` | `validate()`, `validate_and_throw()`, error handling |
| `program` / `main` | Entry point that calls all samples |

Naming conventions by language:

| Language | Convention | Example |
|----------|------------|---------|
| C# | PascalCase `.cs` | `BasicCardSample.cs` |
| TypeScript | camelCase `.ts` | `basicCardSample.ts` |
| Python | snake_case `.py` | `basic_card_sample.py` |
| Go | snake_case `.go` | `basic_card_sample.go` |

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

### Constraints (dotnet)
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
- Library lives in `node/packages/fluent-cards/src/`. Tests live in `node/packages/fluent-cards/tests/`.

### Guardrails
- This library implements the **Adaptive Cards 1.6.0 specification**. All elements, properties, actions, and enums must conform to the schema.
- Use TypeScript string enums (e.g., `TextSize.ExtraLarge = 'extraLarge'`) — they serialize correctly without custom converters.
- Use interfaces + discriminated unions instead of class hierarchies; type narrowing via `element.type === 'TextBlock'`.
- Use `undefined` for optional fields — `JSON.stringify` omits them automatically.
- Builder methods return `this` for fluent chaining; `build()` returns a plain model object.

### Constraints (node)
- Keep diffs minimal and scoped to the request.
- Update or add tests for any behavior change.
- Do not modify CI, dependency versions, or security settings unless asked.
- Never print, log, or commit secrets.

---

## python/

### Verification
```
cd python
pip install -e ".[dev]"
pytest
```

### Environment
- Python 3.10+. No third-party runtime dependencies — pure stdlib only.
- Library lives in `python/src/fluent_cards/`. Tests live in `python/tests/`. Samples in `python/samples/`.
- Builder classes are in `python/src/fluent_cards/builders/` (inputs in a sub-folder).
- Public API is re-exported from `python/src/fluent_cards/__init__.py` — always update `__all__` when adding public symbols.

### Guardrails
- This library implements the **Adaptive Cards 1.6.0 specification**. All elements, properties, actions, and enums must conform to the schema.
- Enums use **PascalCase** members (e.g., `TextSize.Large`, `TextWeight.Bolder`). Do not use UPPER_CASE.
- `build()` returns a plain `dict` — there are no model objects. Serialization is done via the module-level `to_json(card)` function, not a method on the result.
- Validation is exposed via module-level `validate(card) -> list[ValidationIssue]` and `validate_and_throw(card)` — there is no `card.validate()` method.
- Follow the builder pattern: `AdaptiveCardBuilder.create() → .with_x() / .add_x(lambda b: b...) → .build()`.
- All lambdas passed to builder methods receive and return their own builder type (e.g., `add_text_block(lambda tb: tb.with_text(...).with_wrap(True))`).

### Constraints (python)
- Keep diffs minimal and scoped to the request.
- Update or add tests for any behavior change.
- Do not modify CI, dependency versions, or security settings unless asked.
- Never print, log, or commit secrets.

---

## go/

### Verification
```
cd go
go build ./...
go test ./...
```
To run samples:
```
cd go/samples
go run .
```

### Environment
- Go 1.22+. Test dependency: `github.com/stretchr/testify`.
- Library lives in `go/fluentcards/` (package `fluentcards`). Tests are in the same package (`_test.go` files). Samples live in `go/samples/` as a separate `main` package.

### Guardrails
- This library implements the **Adaptive Cards 1.6.0 specification**. All elements, properties, actions, and enums must conform to the schema.
- Enums are typed string constants using the pattern `TypeName + MemberName` (e.g., `TextSizeLarge`, `TextWeightBolder`, `TextColorAccent`).
- `Build()` returns a plain `map[string]any`. Serialization is done via `fluentcards.ToJSON(card)` which returns `(string, error)`.
- Validation is exposed via `fluentcards.Validate(card)` and `fluentcards.ValidateAndPanic(card)`.
- Builder pattern: `NewAdaptiveCardBuilder() → .WithX() / .AddX(func(b *XBuilder) {...}) → .Build()`.
- Use `defer` + `recover()` in samples to handle panics from `ValidateAndPanic` — re-panic on unexpected types.
- Do not use `interface{}` — prefer `any` (Go 1.18+ alias).

### Constraints (go)
- Keep diffs minimal and scoped to the request.
- Update or add tests for any behavior change.
- Do not modify CI, dependency versions, or security settings unless asked.
- Never print, log, or commit secrets.
