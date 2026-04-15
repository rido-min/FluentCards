# Project Context

- **Owner:** rido-min
- **Project:** FluentCards — multi-language library for building Adaptive Cards using fluent builder patterns with strong typing and schema validation
- **Stack:** C#/.NET 8, TypeScript/Node.js, Python 3.10+, Go 1.22+ (prototyping)
- **Scope:** Cross-language test parity across dotnet/, node/, python/
- **Schema:** Adaptive Cards 1.6.0
- **Created:** 2026-04-15

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### 2026-04-15 — Initial Test Suite Review

- **.NET is the reference port** with 583 tests across 46 files. TypeScript has 102, Python 104, Go 63. The gap is ~5.6×.
- **TS and Python test suites are near-mirrors** of each other — same 7 files, same test names (adapted to language conventions), nearly identical counts. Changes to one should always be mirrored to the other.
- **Go is the thinnest port** — 63 tests in 7 files. Validation coverage is notably behind even TS/Python.
- **.NET-only test areas** (no equivalent elsewhere): schema conformance (37 tests), version parsing (21+49 tests), authentication (8), refresh (7), fallback (8), requires (8), styling properties (11), input-specific deep tests (84 across 8 files), serialization edge cases/converters (40+), edge cases (7), sample validation (16), integration tests (5).
- **Validation tests are the strongest parity area** — .NET 54, TS 29, Python 29, Go 13. TS/Python cover the same 29 rules.
- **Teams template tests exist in all 4 ports** — closest to full parity.
- **TS file naming issue:** `actions.test.ts` actually tests card/text builders; `builder.test.ts` actually tests action builders. Swapped names.
- **Test frameworks:** xunit (.NET), node:test with tsx (TS), pytest (Python), testing + testify (Go).
- **Priority parity gaps:** schema conformance > input deep tests > serialization edge cases > version/auth/refresh/fallback > integration tests.

### 2026-04-15: Coordinated Full-Team Codebase Review

Participated in comprehensive multi-port assessment led by Keaton with Fenster (TS), McManus (.NET), Hockney (Python). Verbal produced detailed test parity analysis showing .NET as clear reference (583 tests). Key finding: schema conformance tests are missing entirely from TS/Python/Go — this is the single biggest test parity gap and must be addressed first. Other gaps: input-specific deep tests (TS/Python ~10 vs .NET 84), serialization edge cases and converters (.NET-only), version/auth/refresh/fallback tests (.NET-only), integration tests (.NET-only). TS/Python mirror structure makes future parity work efficient — batching changes across both recommended. Go notably thinner and would benefit from validation test expansion (13 vs 29 elsewhere). Compiled complete matrix and prioritized recommendations in verbal-codebase-review.md.
