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
