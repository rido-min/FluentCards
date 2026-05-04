# Project Context

- **Owner:** rido-min
- **Project:** FluentCards — multi-language library for building Adaptive Cards using fluent builder patterns with strong typing and schema validation
- **Stack:** C#/.NET 8, TypeScript/Node.js, Python 3.10+, Go 1.22+ (prototyping)
- **Core ports:** dotnet/, node/, python/ (primary); go/ (prototyping)
- **Schema:** Adaptive Cards 1.6.0 (`https://adaptivecards.io/schemas/1.6.0/adaptive-card.json`)
- **Created:** 2026-04-15

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### Codebase Architecture Review Findings

- **Builder coverage is complete across all four ports.** Every port implements builders for all 18 element/input types, plus Action, BackgroundImage, Refresh, Authentication, and TextRun. File structure is consistent: dotnet uses `{Type}Builder.cs`, node uses `{Type}Builder.ts`, python uses `{type}_builder.py`, go uses `{type}_builder.go`.
- **Enum parity is achieved.** All ports define the same ~20 schema enums. Only .NET has an extra `AdaptiveCardVersion` enum not present elsewhere.
- **Sample parity is achieved.** All four ports have the same 7 sample files (basic_card, form_card, layout_card, people_picker, rich_content, validation, program/main).
- **Highest-priority schema gap:** .NET `TextBlock` model (`dotnet/src/FluentCards/TextBlock.cs`) is missing `SelectAction` — a property defined in the Adaptive Cards 1.6.0 spec and supported by Node, Python, and Go.
- **Builder method gap in .NET:** `TextBlockBuilder.cs` does not expose `WithSpacing`, `WithSeparator`, `WithIsVisible`, or `WithSelectAction` even though the base model `AdaptiveElement.cs` has the backing properties for the first three. Node/Python/Go builders all expose these.
- **Builder entry point:** dotnet/node/python use `Create()` static factory; Go uses `NewAdaptiveCardBuilder()` (idiomatic Go).
- **Callback vs pre-built pattern split:** dotnet and Go use callback-builder for `WithBackgroundImage`/`WithSelectAction` on AdaptiveCardBuilder. Node and Python accept pre-built objects/dicts instead.
- **Go naming:** `WithRTL` (all-caps acronym, Go convention), `TeamsCards` (not `TeamsAdaptiveCards`), `ValidateAndPanic` (not `ValidateAndThrow`). These are language-idiomatic but differ from other ports.
- **README inconsistency:** dotnet README is 48 lines, Python 143, Go 160, Node has duplicate READMEs. No shared template.
- **Serialization paths:** dotnet: `AdaptiveCardExtensions.ToJson/FromJson` + full `AdaptiveCardSerializer` class. Node: `toJson/fromJson`. Python: `to_json/from_json`. Go: `ToJSON/FromJSON/ToJSONIndent`.
- **Validation paths:** dotnet: `Validation/AdaptiveCardValidator.cs`. Node: `validation.ts`. Python: `validation.py`. Go: `validation.go`. Core checks are aligned; .NET is stricter on root type validation.
- **Teams helpers:** All 4 ports implement the same 5 Teams card templates (Approval, StatusUpdate, TaskUpdate, MeetingReminder, ExpenseReport).

### 2026-04-15: Coordinated Full-Team Codebase Review

Conducted comprehensive multi-port codebase review with all five squad agents. Keaton led architecture review across all ports while Fenster (TS), McManus (.NET), Hockney (Python), and Verbal (tests) went deep on their specialties. Key cross-port findings: TextBlock.selectAction missing in .NET (critical), Column properties incomplete across ports, ActionBuilder behavior inconsistency, build() mutability footgun, schema conformance tests missing from all non-.NET ports. Full details in orchestration logs, session log, and compiled suggestions (squad-codebase-suggestions.md in decisions/inbox/). Team consensus needed on prioritization; .NET TextBlock fix recommended as first task.

### 2026-04-15: Comprehensive Schema Conformance Audit (Post-PR 57)

Performed exhaustive conformance audit of .NET port against Adaptive Cards 1.6.0 specification (all 100,000+ chars of schema JSON). **Result: EXCELLENT CONFORMANCE — Zero critical gaps found.** The earlier findings (TextBlock.selectAction, Column properties, Action.Submit/Execute) have all been addressed by recent PRs. Current state: All 16 element types ✅, all 5 action types ✅, all 17 enums ✅, all advanced features (Refresh, Authentication, Metadata, captionSources, choices.data) ✅, complete builder coverage ✅. False alarms: Container.rtl and TableCell.rtl flagged initially but verified as implemented (base class inheritance + schema typo "rtl?"). One low-priority enhancement opportunity: BackgroundImage string shorthand (schema allows string OR object; .NET only supports object form, which is more type-safe). Conclusion: PR 57 closed the last conformance gaps. Library is production-ready and fully spec-compliant across all versions 1.0–1.6. Full audit report: .squad/decisions/inbox/keaton-schema-conformance-audit.md. Methodology: fetched official schema, systematic definitions/* comparison, line-by-line property verification, builder coverage review. Confidence: Very High.

### 2025-07-22: Python Port Schema Conformance Audit

Performed full conformance audit of the Python port against the Adaptive Cards 1.6.0 schema. **Result: FAIL — 7 gaps found (3 Medium, 4 Low, 0 Critical).** Python shares all 8 gaps originally found in the TS port (PR #67): TextRun missing fontType, Media missing captionSources, CaptionSource model absent, TextBlockBuilder missing height/fallback/requires, AssociatedInputs enum uses lowercase instead of PascalCase, ColumnBuilder.with_width() type hint is str-only (schema allows string|number), no BlockElementHeight enum. No unique Python-only gaps found beyond these shared issues. Design notes: TextBlock.selectAction is a deliberate extension beyond schema; with_rtl() is over-broad on many element builders where schema doesn't define it. Full audit report: .squad/decisions/inbox/keaton-python-schema-audit.md. Confidence: Very High.
### 2025-07-22: TypeScript Port — Full Schema Conformance Audit

Performed full schema conformance audit of the TypeScript port (`node/packages/fluent-cards/src/`) against the Adaptive Cards 1.6.0 specification. **Result: FAIL — 8 actionable gaps found, no critical blockers.**

Key findings:
- **TextRun missing `fontType`** — property absent from model and builder (Medium)
- **Media missing `captionSources`** — v1.6 feature not implemented; `CaptionSource` interface entirely absent (Medium)
- **Column.width and TableColumnDefinition.width typed as `string` only** — schema allows `string | number` for relative weights (Medium)
- **TextBlockBuilder missing 4 base element methods** — `withHeight()`, `withFallback()`, `withRequires()`, `withRtl()` present in all other element builders (Low)
- **AssociatedInputs enum uses camelCase** (`'auto'`/`'none'`) but schema canonical values are PascalCase (`'Auto'`/`'None'`); schema regex allows both (Low)
- **TextBlock.selectAction** is an intentional team extension not in the 1.6.0 schema — documented as design note
- All 5 action types, all 6 input types, all advanced features (Auth, Refresh, Metadata) are fully conformant
- 17 of 18 enums match schema exactly
- Existing test file has 21 tests; ~60 more needed for .NET parity (~84 tests)
- Full audit report: `.squad/decisions/inbox/keaton-ts-schema-audit.md`

### 2026-04-27 — Python 3.8+ Support Complete

- Coordinated with Hockney on Python minimum version support initiative.
- Expanded CI test matrix in `.github/workflows/ci.yml` from `[3.10, 3.12]` to `[3.8, 3.9, 3.10, 3.11, 3.12, 3.13]`.
- Hockney completed audit confirming zero code changes needed; `pyproject.toml` constraint updated.
- Documentation: Decision merged into `.squad/decisions.md`, orchestration logs completed.
- Team impact: Unblocks downstream consumers requiring Python 3.8/3.9.
- Future guardrail: Documented that contributors must maintain `from __future__ import annotations` and avoid 3.9+ stdlib APIs.

### 2025-01-23 — Issue #80 Architecture Plan: Deno Support via Dual Publication

- Produced comprehensive architecture plan for adding Deno support to TypeScript port.
- **Key decision:** Dual publication to npm + JSR from single `node/` codebase — not a new top-level port.
- **Rationale:** Deno is a runtime target, not a new language per AGENTS.md "Adding a New Language Port" rules. TypeScript source is 100% Deno-compatible (zero Node.js APIs detected via grep audit). Dual publication minimizes maintenance burden, avoids sample drift, and aligns with JSR single-source-of-truth philosophy.
- **Technical approach:** Add `jsr.json`, ESM build config (`tsconfig.jsr.json`), Deno samples in `node/samples/deno/` (snake_case, JSR imports), optional `deno test` suite, CI integration with version stamping.
- **Sample parity strategy:** Deno entry points in `node/samples/deno/` call shared sample logic from `node/samples/` — no duplication of 7 canonical samples. AGENTS.md parity rules apply: changes to Node samples must update Deno entry points.
- **Work breakdown:** 8 tasks for Fenster (TS Dev) and Verbal (Tester) covering config, samples, tests, CI, docs, secrets, manual publish test, validation.
- **Open questions:** JSR scope name confirmation (`@rido-min/fluent-cards` vs `@fluent-cards/core`), Deno test scope (20+ core tests vs full 280 parity), publish cadence (sync with npm vs manual).
- **Schema impact:** Zero — publication target change does not affect Adaptive Cards 1.6.0 conformance.
- **Ecosystem precedent:** Modern TS libraries (Zod, Effect, tRPC) ship to both npm + JSR from single codebase; this is established pattern.
- **Status:** Planning completed 2026-05-04. Merged into decisions.md as active decision. Awaiting user input on 3 open questions before Fenster + Verbal execution begins.
- Architecture plan: `.squad/decisions/inbox/keaton-issue-80-deno-plan.md` (merged to decisions.md on 2026-05-04)

### 2026-05-04 — Issue #80: Post-Implementation Learnings and Rethink

Fenster's implementation (PR #81) used `build-jsr.mjs` to rewrite `.js` → `.ts` extensions because JSR appeared to reject `.js` extensions even with `--sloppy-imports`. **Post-implementation investigation revealed this was a configuration error, not a JSR limitation.**

**CRITICAL EMPIRICAL FINDING (2026-05-04 architecture rethink):**
- JSR DOES accept `.js` extensions in TypeScript source when `"unstable": ["sloppy-imports"]` is configured at the **workspace root** (`node/deno.json`), not the package level.
- This configuration allows:
  1. `deno publish --dry-run` to succeed with `jsr.json` pointing at `./src/index.ts` directly (no jsr-src/ rewrite)
  2. `deno test tests-deno/` to run without `--sloppy-imports` flag
  3. Local Deno samples to import from `../../packages/fluent-cards/src/index.ts` and execute without flags
- All three test vectors passed empirically (Deno 2.7.14, 2026-05-04).

**Revised recommendation:** Use workspace-root `deno.json` with sloppy-imports instead of the build-jsr.mjs rewrite. This eliminates the two-source-of-truth problem, removes 5 seams (build script, negative glob, --allow-dirty workaround, jsr-src/ directory, per-command --sloppy-imports), and allows samples to run locally without waiting for JSR publish.

**Lesson for future JSR projects:** Always test workspace-root `deno.json` configuration before reaching for build-time transformations. Deno's documentation states sloppy-imports "can only be specified in the workspace root" — this is a hard requirement that was missed during PR #81 implementation.

Full architecture comparison and migration plan: `.squad/decisions/inbox/keaton-issue-80-rethink.md`
