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

### 2026-04-27 — Deno Compatibility Audit (Issue #80)

Conducted comprehensive Deno/JSR readiness audit of TypeScript port for Keaton. Key findings:

**Green light:** Production code is 100% Deno-ready — zero Node built-ins, zero runtime deps, pure ESM source, all 132 relative imports already have explicit `.js` extensions (discovered library was already compliant — no migration needed for import paths).

**Two blockers:**
1. `tsconfig.json` emits CommonJS (`"module": "CommonJS"`) — must switch to `"module": "ES2022"` or `"ESNext"` for JSR publication
2. `package.json` missing `"type": "module"` declaration — trivial 1-line add

**Tests:** 12 test files use `node:test` + `node:assert/strict` — would need Deno test adapter or dual test suite for `deno test` compatibility. Does not block JSR publication (tests aren't published). Production library has no Node dependencies.

**Migration effort:** Low — 2-4 hours for Phase 1 (JSR-ready). Library architecture is already Deno-compatible by design (zero deps, explicit extensions, ESM-only source). Full report in `.squad/decisions/inbox/fenster-issue-80-deno-audit.md` for Keaton's review. Audit merged to decisions.md on 2026-05-04 as active decision.

### 2026-05-04 — JSR Publication Implementation (Issue #80)

Implemented dual-publication support for npm and JSR (Deno registry) on branch `squad/80-deno-support`. Key constraint: **must NOT break npm CommonJS build**.

**Architecture decision:** JSR consumes `src/*.ts` directly via `jsr.json`'s `exports` field. No compiled output for JSR. This avoids the `"type": "module"` trap that would break npm's CommonJS consumers.

**Critical insight:** The two blockers from the audit (tsconfig module, package.json type) were **both avoided** by having JSR read source directly. npm build stays CommonJS, JSR gets native ESM by consuming `.ts` files.

**Files created:**
- `node/packages/fluent-cards/jsr.json` — JSR package config with scope `@adaptivecards/fluent`
- `node/packages/fluent-cards/LICENSE` — Copied from root for JSR publication
- `node/samples/deno/` — 7 canonical samples + program.ts (snake_case, `import.meta.main` pattern)
- `node/samples/deno/README.md` — Deno sample documentation

**Files modified:**
- `.github/workflows/ci.yml` — Added Deno setup, `deno check`, dry-run publish (PRs), JSR publish (tags with version stamping)
- `README.md` — Added TypeScript/Deno row to Language Ports table and Quick Start example
- `AGENTS.md` — Documented Deno support pattern under `node/` section (dual-publication, constraints, sample naming)
- `node/packages/fluent-cards/README.md` — Added Deno installation instructions and sample links

**Validation:**
- npm build: ✅ Clean (CommonJS unchanged)
- npm test: ✅ All tests pass
- npm typecheck: ✅ Clean
- Deno dry-run: Skipped locally (CI will validate)
- Commit SHA: `f27b4e280bb93242f58ee7db6c0ad28773cd612d`
- Branch: `squad/80-deno-support` (pushed to origin)

**Deferred:**
- `DENO_DEPLOY_TOKEN` secret — rido must add to repo settings for CI JSR publish
- Deno test suite — Verbal will add ~20 core tests in follow-up commit (AdaptiveCardBuilder, builders, toJson/fromJson, validate, round-trip)
- PR creation — Coordinator opens after Verbal's tests merge

### 2026-05-04 — Issue #80: JSR Dual-Publication Implementation (Completed)

Successfully implemented Deno support via dual publication to npm and JSR from single TypeScript codebase. Key learning: **JSR strict module resolution rejects `.js` extensions** — must use build-jsr.mjs script to transform extensions before JSR operations. This is critical for any future JSR adoption.

**Architecture:**
- `jsr.json` exports point to `jsr-src/index.ts` (build-generated, gitignored)
- `scripts/build-jsr.mjs` transforms npm source (`.js` exts) → JSR source (`.ts` exts)
- npm CommonJS build remains unchanged (no `"type": "module"` added)
- Deno samples in `node/samples/deno/` (snake_case)
- 39 core tests in `tests-deno/` (Verbal delivered)
- CI integration with version stamping, JSR publish on tags

**Commits:** 678262b, 1bc6f11, c16b259 (Fenster) + 4179700 (Verbal) + c69878e (Coordinator patch)

**Key constraint:** `--sloppy-imports` required for local deno tests (source uses .js exts); resolved by build-jsr.mjs before JSR publication.

**Critical insight not in original plan:** JSR's strict analysis cannot be bypassed with `--no-check`. The build-jsr pattern is essential, not optional.

**Status:** Ready for PR merge. Awaiting `DENO_DEPLOY_TOKEN` secret and team approval. Build-jsr pattern documented in `.squad/skills/jsr-dual-publication/SKILL.md` for reuse.

