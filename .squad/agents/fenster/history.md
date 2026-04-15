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
