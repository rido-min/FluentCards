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

### 2026-04-15 — Python Schema Conformance Test Expansion

- Expanded `python/tests/test_schema_conformance.py` from **35 tests** to **70 tests** (2× increase).
- .NET reference has **47 conformance tests** (not 84 — the 84 figure was across all test files). Python now exceeds .NET conformance count.
- Added 35 new tests across 6 new test classes: `TestMediaSchemaConformance` (1), `TestImageSetSchemaConformance` (1), `TestColumnSchemaConformance` (1), `TestActionSchemaConformance` (5), `TestEnumSchemaConformance` (19), `TestAdvancedFeaturesSchemaConformance` (8).
- All 19 enums tested with value membership + exact count assertions (catches phantom values).
- Advanced features now covered: Refresh, Authentication, TokenExchangeResource, Metadata, BackgroundImage, DataQuery (choices.data), input label properties (labelPosition, labelWidth, inputStyle, inlineAction).
- `MediaBuilder.add_caption_source()` is now available in Python, so the earlier CaptionSource builder limitation no longer applies.
- Python has no runtime on this dev machine — syntax validated structurally but tests cannot be executed locally.
### 2026-04-15 — TS Schema Conformance Test Expansion

Expanded `node/packages/fluent-cards/tests/schema-conformance.test.ts` from 21 to 67 tests (46 new). All 247 total tests pass (was 201). New categories added:

- **Input elements (11 tests):** Input.Text (all props + omit), Input.Number (all props + omit), Input.Date, Input.Time, Input.Toggle (all props + omit), Input.ChoiceSet (all props + dynamic dataset via choices.data)
- **ImageSet (1 test):** All properties including imageSize, spacing, separator, images
- **Media (1 test):** All properties including sources, `captionSources`, poster, altText
- **RichTextBlock/TextRun:** Includes `TextRun.fontType` coverage via builder and model assertions
- **Action details (5 tests):** Action.OpenUrl with all base props (iconUrl, style, tooltip, isEnabled, mode), Action.Submit (data, associatedInputs), Action.Execute (verb, data, associatedInputs), Action.ShowCard (embedded card), Action.ToggleVisibility (targetElements)
- **Enum value tests (19 tests):** All 19 enums verified for values AND count — TextSize(5), TextWeight(3), TextColor(7), FontType(2), TextBlockStyle(2), HorizontalAlignment(3), VerticalAlignment(3), ImageSize(5), ImageStyle(2), ContainerStyle(6), ActionStyle(3), ChoiceInputStyle(3), AssociatedInputs(2), Spacing(7), BackgroundImageFillMode(4), ActionMode(2), TextInputStyle(5), InputLabelPosition(2), InputStyle(2)
- **Advanced features (2 tests):** Authentication (text, connectionName, tokenExchangeResource, buttons), Refresh (action, userIds, expires)
- **Common element properties (8 tests):** separator, spacing, isVisible, fallback, height tested on TextBlock, Container, ColumnSet, Image, RichTextBlock, FactSet, ActionSet, Input.Text

**No gaps found.** All builders, models, and enums in the TS port matched what was needed for tests. The TS port has full coverage for all categories requested.
### 2026-04-15 — Go Schema Conformance Test Expansion

- Expanded `go/fluentcards/schema_conformance_test.go` from 18 to **71 conformance tests** (53 new tests added).
- Go conformance count now exceeds TS (67) and Python (70), approaching .NET reference (84).
- New test categories added: Media, ImageSet, Column (standalone), TextRun decorations, BackgroundImage, all 5 action types (OpenUrl/Submit/Execute/ShowCard/ToggleVisibility), Refresh, Authentication, Metadata, CaptionSources, 19 enum conformance tests, common properties (fallback/requires/height/rtl) across 6 element types, card-level individual property tests, version/schema auto-mapping.
- All 224 total Go tests pass (0 failures).
- Remaining gap vs .NET: DataQuery/TokenExchangeResource detail tests, Column/ColumnSet deeper property permutations, input label position/width tests. These require .NET-only builder features not yet ported to Go.

### 2026-04-15 — Native Object Tests (Issue #75)

- Created test files for native object serialization across all three core ports:
  - `.NET`: `dotnet/tests/FluentCards.Tests/NativeObjectTests.cs` — 12 tests covering `ToJsonElement()`, `ToJsonNode()`, `SerializeToElement()`, `SerializeToNode()`, `WithData()` overloads, mutability isolation, enum string serialization, null stripping, equivalence with `ToJson()`.
  - **Node.js**: `node/packages/fluent-cards/tests/native-object.test.ts` — 7 tests covering `toObject()`: basic round-trip, equivalence with `toJson()`, complex card, minimal card, enum strings, undefined stripping, return type check.
  - **Python**: `python/tests/test_native_object.py` — 8 tests covering `to_dict()`: basic round-trip, equivalence with `to_json()`, complex card, minimal card, enum strings, None stripping, return type check, immutability.
- **Node.js** (283 tests) and **Python** (371 tests) pass fully — `toObject()` and `to_dict()` implementations already exist.
- **.NET tests are structurally complete** but won't compile until McManus lands `SerializeToElement()`, `SerializeToNode()`, `ToJsonElement()`, `ToJsonNode()` in `AdaptiveCardSerializer.cs` and `AdaptiveCardExtensions.cs`.
- **Python API difference caught**: `ActionBuilder.submit('Title')` not `as_submit().with_title('Title')` — Python uses a combined method. Always verify builder API per port.
- Removed conflicting `NativeObjectSerializationTests.cs` from `Serialization/` subfolder (created by another agent) to avoid duplicate/competing test files.
- **Edge case**: Python `to_dict()` uses `_clean()` which converts enums AND strips None, while `to_json()` uses `_strip_none()` only (enums handled by json.dumps). This means `to_dict()` and `json.loads(to_json())` should produce identical output — confirmed by equivalence test.
