# Squad Decisions

## Active Decisions

No decisions recorded yet.

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
