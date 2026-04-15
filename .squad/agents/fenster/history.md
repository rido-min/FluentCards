# Project Context

- **Owner:** rido-min
- **Project:** FluentCards — multi-language library for building Adaptive Cards using fluent builder patterns with strong typing and schema validation
- **Stack:** C#/.NET 8, TypeScript/Node.js, Python 3.10+, Go 1.22+ (prototyping)
- **Port:** node/ (TypeScript/Node.js)
- **Schema:** Adaptive Cards 1.6.0
- **Created:** 2026-04-15

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### 2025-07-16 — Initial Codebase Review

- **Structure:** Library in `node/packages/fluent-cards/src/`, tests in `tests/`, samples in `node/samples/`. Workspace monorepo with root `node/package.json`.
- **Core files:** `models.ts` (interfaces + discriminated unions), `enums.ts` (17 string enums), `serialization.ts` (thin JSON.stringify/parse wrappers), `validation.ts` (~420 lines, version-aware).
- **Builders:** 17 builder classes in `src/builders/` + 6 input builders in `src/builders/inputs/`. All class-based, return `this`, `build()` returns the internal model object (not a copy).
- **Tests:** 102 tests across 7 files, all passing. Typecheck clean. Coverage tool `c8` configured.
- **Samples:** All 7 canonical samples present per AGENTS.md. `program.ts` is the entry point.
- **TeamsAdaptiveCards:** Extra factory module with 5 Teams-pattern card templates.
- **Key concern:** `build()` returns mutable internal reference — reuse/multi-build is a footgun.
- **Key concern:** `Column` doesn't extend `AdaptiveElementBase`, missing several spec properties.
- **Key concern:** Nested builders (Container, Column) don't expose all element types that `AdaptiveCardBuilder` does.
- **Naming inconsistency:** `TextRunBuilder` uses `isSubtle()` while `TextBlockBuilder` uses `withIsSubtle()`. `InputChoiceSetBuilder.isMultiSelect()` also breaks `withX()` convention.

### 2026-04-15: Coordinated Full-Team Codebase Review

Participated in comprehensive multi-port review led by Keaton with Fenster (TS), McManus (.NET), Hockney (Python), Verbal (tests). Focused on TypeScript-specific issues: package.json export field gaps (missing ESM), loose Node version requirement (should be >=20), Column interface incompleteness, naming inconsistencies across builders, validation rigor, and nested builder element coverage. Compiled into fenster-codebase-review.md. Cross-port consensus: build() mutability is a footgun, ActionBuilder silent no-op needs fixing, Column properties must be audited and fixed across all ports. Test parity gaps documented by Verbal show TS needs schema conformance and integration tests alongside other ports.

### 2025-07-22 — Schema Conformance Fixes (Keaton Audit)

Fixed all 8 gaps from Keaton's TS schema audit plus the enum casing fix:

1. **CaptionSource interface** added to `models.ts` — new v1.6 type with `type`, `mimeType`, `url`, `label`.
2. **Media.captionSources** added to `Media` interface.
3. **TextRun.fontType** added (`FontType` enum).
4. **Column.width & TableColumnDefinition.width** widened from `string` to `string | number` (numeric relative weights).
5. **TextRunBuilder.withFontType()** added, imports `FontType`.
6. **MediaBuilder.addCaptionSource()** added — creates `CaptionSource` objects inline.
7. **TextBlockBuilder** got `withHeight()`, `withFallback()`, `withRequires()` — matched ContainerBuilder/MediaBuilder pattern.
8. **ColumnBuilder.withWidth()** and **ColumnSetBuilder.addColumn()** width params updated to `string | number`.
9. **AssociatedInputs enum** values changed to PascalCase (`'Auto'`, `'None'`) to match schema canonical values. Updated all affected test assertions.

- `CaptionSource` exported from `index.ts`.
- Library source typechecks clean. All 247 tests pass.
- Pre-existing test typecheck issues (68 errors from `values.includes('literal')` pattern in enum tests) were not introduced by these changes.

### 2025-07-23 — Issue #75: toObject() for native object serialization

- Added `toObject(card)` to `serialization.ts` — returns a clean `AdaptiveCard` object with all `undefined` values recursively stripped via a `stripUndefined` helper.
- Exported from `index.ts` alongside `toJson` and `fromJson`.
- 6 new tests in `serialization.test.ts` — stripping, immutability, array preservation, parity with `JSON.parse(toJson())`.
- Updated `program.ts` sample to demonstrate `toObject()`.
- **Key pattern:** Tests resolve imports from `dist/` (CJS via tsx), so `npm run build` must run before `npm test` when adding new exports. The workspace `npm install` does not auto-build.
- All 283 tests pass. Typecheck clean.

### 2026-04-15 — Native Object Serialization (#75) — Cross-Team Coordination

Collaborated with McManus (.NET), Hockney (Python), and Verbal (Tester) on Issue #75. TypeScript implementation complete with 6 new tests in `native-object.test.ts`. All three core ports (dotnet, node, python) now provide native object methods: .NET `ToJsonElement()`/`ToJsonNode()`, TypeScript `toObject()`, Python `to_dict()`. Test parity maintained — all ports cover identical semantic scenarios (round-trip, equivalence, complex card, minimal card, enum strings, field stripping). Verbal's cross-port test framework ensures all implementations produce bit-identical results to `JSON.parse(toJson())`. Go skipped pending architecture review (`go:needs-research`).
