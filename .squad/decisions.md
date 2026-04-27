# Squad Decisions

## Active Decisions

### 2026-04-15: Schema Conformance Audit — .NET Port vs Adaptive Cards 1.6.0
**Status:** Complete  
**Auditor:** Keaton (Lead Architect)  
**Result:** ✅ PASS — No critical gaps found  
**Confidence:** Very High

#### Executive Summary
The .NET port demonstrates **exceptional schema conformance** to the Adaptive Cards 1.6.0 specification. All 16 element types, 5 action types, 6 input types, 17 enums, and all advanced features (Refresh, Authentication, Media captions, dynamic data) are fully implemented. PR 57 closed the final conformance issues (Action.Submit/Execute), and the library is now **production-ready**.

#### Key Findings
| Category | Count | Status |
|----------|-------|--------|
| Conformance Notes | 16 | ✅ Positive |
| Enhancement Opportunities | 1 | Low (Optional) |
| False Alarms | 2 | Low (Already Fixed) |
| Critical Gaps | 0 | None |
| Medium Gaps | 0 | None |

#### Specific Risk Areas — All Clear
- ✅ TextBlock.selectAction (present, line 69)
- ✅ Column properties (all 14 present)
- ✅ Action base properties (all 9 present)
- ✅ Input base properties (all 7 present)
- ✅ AdaptiveCard top-level (all 13 present)
- ✅ Table/TableRow/TableCell (full v1.5 support)
- ✅ Authentication & Refresh (all properties)
- ✅ Media.captionSources & Input.ChoiceSet.choices.data (v1.6)

#### Enhancement Opportunity (No Action Required)
**BackgroundImage String Shorthand:** The schema allows BackgroundImage as either a string (shorthand) or object. The .NET port only supports the object form, which is more explicit and type-safe. Recommendation: keep as-is.

#### Audit Methodology
1. Fetched official schema from `https://adaptivecards.io/schemas/1.6.0/adaptive-card.json`
2. Systematically compared all definitions against .NET models
3. Verified builders for property coverage
4. Validated enums and advanced features
5. Cross-checked 25+ model files, 23 builders, 17 enums

#### Conclusion
**No action required** — the .NET port is production-ready. Mark as baseline for future schema updates.

---

### 2026-04-15: Issue #75 — Native Object Serialization Methods
**Status:** Implemented  
**Participants:** McManus (.NET), Fenster (TypeScript), Hockney (Python), Verbal (Tester)  
**Branch:** squad/75-native-object-methods

#### Context
Consumers embedding Adaptive Cards into larger JSON payloads (e.g., Bot Framework activities, Teams messages) were forced to serialize to string via `ToJson()`/`toJson()`/`to_json()` and then re-parse, causing double serialization overhead.

#### Decision
Implement native object serialization methods across all three core ports that apply the same cleanup as their respective `toJson` family (null/undefined/None stripping, enum conversion) but return the native type:

**C# / .NET:**
- `SerializeToElement()` and `SerializeToNode()` static methods on `AdaptiveCardSerializer`
- `ToJsonElement()` and `ToJsonNode()` extension methods on `AdaptiveCard`
- `WithData<T>()` generic overload on `ActionBuilder` for direct serialization of typed data
- All use source-generated `FluentCardsJsonContext` for AOT compatibility

**TypeScript/Node.js:**
- `toObject(card)` module-level function with recursive `stripUndefined` helper
- Never mutates input; skips undefined keys, recursively cleans nested objects/arrays
- Semantically equivalent to `JSON.parse(JSON.stringify(card))` without string allocation

**Python:**
- `to_dict(card)` module-level function with `_clean()` helper
- Strips None values and converts Enum instances to plain strings recursively
- Matches cleanup semantics of `to_json()`

#### Key Design Principles
- Two API surfaces (.NET static + extension) match existing `Serialize`/`ToJson` pattern
- `WithData<T>()` requires explicit `[JsonSerializable]` registration — deliberate AOT constraint
- Module-level functions (TypeScript, Python) align with ecosystem conventions
- All implementations apply identical cleanup logic to their `toJson` counterparts

#### Test Results
- **.NET:** 12 new tests (707 total, was 698)
- **TypeScript:** 7 new tests (283 total, was 277)
- **Python:** 8 new tests (370 total, was 363)
- All tests pass ✅

#### Go Port
Skipped pending architecture decision (`go:needs-research` label). Decision framework applies when Go approach is determined.

---

### 2026-04-27: Lower Python Minimum Version to 3.8+
**Status:** Implemented  
**Author:** Hockney (Python Dev)  
**Issue:** #77  
**PR:** #78

#### Decision

Lowered `requires-python` from `>=3.10` to `>=3.8` with zero library code changes.

#### Rationale

Full audit confirmed the Python codebase was already 3.8-compatible:
- All PEP 604 union syntax (`X | Y`) and built-in generic syntax (`dict[str, Any]`) guarded by `from __future__ import annotations`
- Enums use `str, Enum` base (not `StrEnum` which requires 3.11)
- No `match/case`, `TypeAlias`, `tomllib`, or 3.9+ stdlib APIs

This unblocks `botas` consumers who need fluent-cards on Python 3.8/3.9.

#### Test Results

- All 363 tests pass ✅
- Zero library code changes required

#### Team Impact

- **CI**: Expanded test matrix from `[3.10, 3.12]` to `[3.8, 3.9, 3.10, 3.11, 3.12, 3.13]` per Keaton
- **Future code**: Contributors should maintain `from __future__ import annotations` in all files and avoid 3.9+ stdlib APIs to preserve compatibility

---

## Governance

- All meaningful changes require team consensus
- Document architectural decisions here
- Keep history focused on work, decisions focused on direction

---

## Decision Records

### 2026-04-15: Codebase Review Findings — Architecture, Schema, Testing
**Status:** Recorded for team review  
**Participants:** Keaton, Fenster, McManus, Hockney, Verbal  
**Related artifacts:** squad-codebase-suggestions.md, orchestration logs, session log

#### Compiled Suggestions (52 items)

See `squad-codebase-suggestions.md` for full details. Summary by category:

**General (all languages):** Schema gaps (default version 1.5 vs 1.6, Column missing properties), validation incomplete, builder pattern issues (mutable build(), ActionBuilder no-op), inconsistent README, no shared test fixtures.

**.NET specifics:** Sub-namespace violation, TextBlock missing selectAction (critical), serializer allocation bug, Column inheritance issues, dual validation systems, weak typing.

**TypeScript specifics:** Missing ESM export, loose Node version requirement, Column incompleteness, naming inconsistencies, nested builder gaps.

**Python specifics:** Stale directory, bare Callable hints, exception swallowing, missing py.typed marker.

**Go specifics:** NewAdaptiveCardBuilder() vs Create(), test coverage significantly thinner, validation tests lag.

**Test parity:** .NET 583 tests vs TS/Python ~102/104 vs Go 63. Massive gaps in schema conformance, input deep tests, edge cases, integration tests.

#### Key Findings

- **TextBlock.selectAction in .NET is the highest-priority schema violation.**
- **Schema conformance tests are missing in TS/Python/Go** — single biggest test parity gap.
- **All ports share ActionBuilder behavior bug:** silent no-op when methods called before type set.
- **build() mutability footgun:** Can be called twice with mutations affecting both results.
- **.NET is the reference port** for both feature coverage and test coverage.

#### Next Steps (To Be Decided)

1. Prioritize which suggestions to implement (recommend: TextBlock.selectAction first, then schema tests)
2. Assign work across the team
3. Establish timelines and milestones
4. Consider whether test parity work should precede or follow feature fixes

---

### User Directive: Adaptive Cards MCP Server
**Timestamp:** 2026-04-15T03:32:00Z  
**From:** rido-min (via Copilot CLI)  
**Subject:** Tooling availability for Keaton and the team

The Adaptive Cards MCP server (`adaptive-cards-mcp` — https://github.com/VikrantSingh01/adaptive-cards-mcp/) should be made available as a tool for Keaton and the team. It provides:
- Schema validation
- Card generation
- Accessibility checks
- Host compatibility testing (Adaptive Cards v1.6)

**Why:** Requested to enhance team's ability to validate and test cards against the official Adaptive Cards schema.

**Status:** Recorded for future reference. Coordinate with infrastructure team for availability.
