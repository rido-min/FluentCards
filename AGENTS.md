# AGENTS.md

## Project Overview

FluentCards is a multi-language library providing a **fluent builder API** for [Adaptive Cards 1.6.0](https://adaptivecards.io/schemas/1.6.0/adaptive-card.json). Every language port mirrors the same schema, exposes the same builder pattern, and produces the same JSON output — just expressed in idiomatic code for each target.

**MCP tools available:** `.copilot/mcp-config.json` configures `adaptive-cards-mcp` for schema validation, card generation, and accessibility checks.

---

## Repo Structure

Each language port lives in its own top-level folder:

| Folder | Language | Status |
|--------|----------|--------|
| `dotnet/` | C# / .NET 8 | Stable |
| `node/` | TypeScript / Node.js | Stable |
| `python/` | Python 3.10+ | Stable |
| `go/` | Go 1.22+ | Stable |

Shared assets (docs, screenshots, root README) live at the repository root.

---

## Universal Constraints (all ports)

- Keep diffs minimal and scoped to the request — no drive-by refactors.
- Update or add tests for any behavior change.
- Do not modify CI, dependency versions, or security settings unless asked.
- Never print, log, or commit secrets.
- All tests must pass before considering work done.

---

## Builder Pattern Quick Reference

The fluent builder pattern is the heart of every port. Here's how the same operations translate across languages:

| Concern | dotnet | node | python | go |
|---------|--------|------|--------|-----|
| Entry point | `AdaptiveCardBuilder.Create()` | `adaptiveCard()` | `AdaptiveCardBuilder.create()` | `fluentcards.NewAdaptiveCardBuilder()` |
| Set property | `.WithText("Hello")` | `.text("Hello")` | `.with_text("Hello")` | `.WithText("Hello")` |
| Add child | `.AddTextBlock(...)` | `.addTextBlock(...)` | `.add_text_block(...)` | `.WithTextBlock(...)` |
| Build | `.Build()` | `.build()` | `.build()` | `.Build()` |
| Returns | `AdaptiveCard` object | plain JS object | plain `dict` | `map[string]any` |
| Serialization | `JsonSerializer.Serialize(card)` | `JSON.stringify(card)` | `to_json(card)` | `fluentcards.ToJSON(card)` |

### Cross-language naming conventions

| Language | Files | Builder methods | Enums |
|----------|-------|-----------------|-------|
| C# | PascalCase `.cs` | `PascalCase` (e.g. `WithText`, `AddColumn`) | `PascalCase` (e.g. `TextSize.ExtraLarge`) |
| TypeScript | camelCase `.ts` | `camelCase` (e.g. `withText`, `addColumn`) | `PascalCase` with camelCase values (e.g. `TextSize.ExtraLarge = 'extraLarge'`) |
| Python | snake_case `.py` | `snake_case` (e.g. `with_text`, `add_column`) | `PascalCase` members (e.g. `TextSize.Large`). Do NOT use UPPER_CASE. |
| Go | snake_case `.go` | `PascalCase` (e.g. `WithText`, `AddColumn`) | `TypeNameMemberName` (e.g. `TextSizeLarge`, `TextWeightBolder`) |

---

## Enum Naming Reference

When adding a new string-enum property across all ports:

| Language | Example `TextSize` | Example `TextWeight` |
|----------|-------------------|----------------------|
| dotnet | `TextSize.ExtraLarge` | `TextWeight.Bolder` |
| node | `TextSize.ExtraLarge = 'extraLarge'` | `TextWeight.Bolder = 'bolder'` |
| python | `TextSize.ExtraLarge` | `TextWeight.Bolder` |
| go | `TextSizeExtraLarge` | `TextWeightBolder` |

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

## Common Task: Add a New Adaptive Card Element

When the spec introduces a new element (e.g. `Rating`, `Table`) or you need to add a missing one, touch these files in every applicable port:

### dotnet
- `src/Models/{Element}.cs` — model type with properties
- `src/Builders/{Element}Builder.cs` — fluent builder
- `src/FluentCardsJsonContext.cs` — add `[JsonSerializable]` entry
- `tests/FluentCards.Tests/{Feature}Tests.cs` — builder + serialization tests

### node
- `packages/fluent-cards/src/models/{element}.ts` — interface / discriminated union member
- `packages/fluent-cards/src/builders/{element}.ts` — fluent builder
- `packages/fluent-cards/src/index.ts` — re-export
- `packages/fluent-cards/tests/{feature}.test.ts` — builder + serialization tests

### python
- `src/fluent_cards/models/{element}.py` — dataclass / typed dict
- `src/fluent_cards/builders/{element}_builder.py` — fluent builder
- `src/fluent_cards/__init__.py` — update `__all__`
- `tests/test_{feature}.py` — builder + serialization tests

### go
- `fluentcards/{element}.go` — struct type
- `fluentcards/{element}_builder.go` — fluent builder
- `fluentcards/{element}_test.go` — builder + serialization tests

**Cross-port checklist:** add to all four ports unless the user explicitly scopes the work to a subset.

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
| `action_submit_execute_sample` | Action.Execute and Action.Submit with custom verbs/data payloads |
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

---

## node/

### Workspace Layout

`node/` is an npm workspace root. The actual published package lives inside it:

```
node/
├── package.json                 # workspace root (private, "fluent-cards-workspace")
├── packages/fluent-cards/       # the published package
│   ├── package.json             # "name": "fluent-cards"
│   ├── src/                     # library source
│   ├── tests/                   # test files
│   └── dist/                    # compiled JS output (gitignored, built by tsc)
└── samples/                     # sample programs (separate workspace member)
```

Always work in `packages/fluent-cards/` for library changes. The root `package.json` is a thin workspace orchestrator — do not add source files or dependencies there.

### Verification
```
cd node
npm install && npm test && npm run typecheck
```

### Environment
- TypeScript with strict mode enabled.
- Node.js built-in test runner (`node:test`) with `tsx` for TypeScript support.
- Library source lives in `node/packages/fluent-cards/src/`. Tests live in `node/packages/fluent-cards/tests/`.

### Guardrails
- This library implements the **Adaptive Cards 1.6.0 specification**. All elements, properties, actions, and enums must conform to the schema.
- Use TypeScript string enums (e.g., `TextSize.ExtraLarge = 'extraLarge'`) — they serialize correctly without custom converters.
- Use interfaces + discriminated unions instead of class hierarchies; type narrowing via `element.type === 'TextBlock'`.
- Use `undefined` for optional fields — `JSON.stringify` omits them automatically.
- Builder methods return `this` for fluent chaining; `build()` returns a plain model object.

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
