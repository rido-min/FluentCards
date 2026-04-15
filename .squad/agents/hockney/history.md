# Project Context

- **Owner:** rido-min
- **Project:** FluentCards — multi-language library for building Adaptive Cards using fluent builder patterns with strong typing and schema validation
- **Stack:** C#/.NET 8, TypeScript/Node.js, Python 3.10+, Go 1.22+ (prototyping)
- **Port:** python/ (Python 3.10+)
- **Schema:** Adaptive Cards 1.6.0
- **Created:** 2026-04-15

## Learnings

<!-- Append new learnings below. Each entry is something lasting about the project. -->

### 2025-07-22 — Initial Codebase Review

- **Package layout**: src-layout at `python/src/fluent_cards/`. Builders in `builders/`, input builders in `builders/inputs/`. All `__init__.py` exports and `__all__` are clean and up to date.
- **Verification**: `cd python && pip install -e ".[dev]" && pytest` — 104 tests pass on Python 3.14.3.
- **Zero runtime deps** confirmed — `dependencies = []` in pyproject.toml. Dev deps: pytest, pytest-cov, pdoc.
- **Enums**: All use PascalCase members with `str, Enum` pattern. Complete for Adaptive Cards 1.6 spec.
- **Builder pattern**: Consistently `create()` → `with_x()` / `add_x(lambda)` → `build()`. All return `self` for chaining. `build()` returns plain `dict`.
- **Serialization**: `to_json(card, indent=2)` strips `None` values. `from_json(json_str)` validates type == 'AdaptiveCard'.
- **Validation**: `validate()` / `validate_and_throw()` module-level. Checks: missing required fields, invalid URLs, duplicate IDs, version mismatches, select action constraints.
- **Stale directory**: `python/fluent_cards/` is leftover from pre-src-layout — should be cleaned up.
- **Type hints gap**: All builder `configure` parameters use bare `Callable` without type parameters.
- **Test coverage**: 7 test files covering builders, elements, actions, inputs, serialization, validation, and Teams helpers. No unit tests for BackgroundImageBuilder, RefreshBuilder, or AuthenticationBuilder.
- **Key file paths**: `enums.py`, `models.py`, `serialization.py`, `validation.py`, `teams.py` at library root. 18 builder files total.

### 2026-04-15: Coordinated Full-Team Codebase Review

Participated in full-squad review coordinated by Keaton. Hockney focused on Python-specific issues: stale `fluent_cards/` pre-migration directory should be deleted, type hints on Callable parameters are bare (should be `Callable[[TextBlockBuilder], None]` for IDE support), `AdaptiveCardBuilder.with_background_image` takes raw dict instead of lambda pattern (inconsistent with ContainerBuilder), `ActionBuilder` silently no-ops before type set, `from_json()` swallows all exceptions (should catch JSONDecodeError specifically), `FactSetBuilder.add_fact` value parameter inconsistency, missing `py.typed` marker (violates PEP 561), old Optional import style, `_strip_none` doesn't strip empty strings, missing unit tests for helper builders. Compiled into hockney-codebase-review.md. Cross-port finding: Python and TypeScript test suites are near-mirrors (same structure, identical coverage counts) — future parity work should batch TS/Python together.

### 2026-04-15 — Fix 7 Python Schema Conformance Gaps

Fixed all 7 gaps identified by Keaton's audit (mirroring TS PR #67):
1. **TextRunBuilder.with_font_type()** — added, imports FontType from enums.
2. **MediaBuilder.add_caption_source()** — added with setdefault pattern for captionSources list.
3. **CaptionSource model** — added TypedDict to models.py, exported from `__init__.py`.
4. **TextBlockBuilder base element methods** — added `with_height()`, `with_fallback()`, `with_requires(key, version)` matching the established `(key, version)` pattern used by all other builders (not `dict` as initially suggested in task).
5. **AssociatedInputs casing** — changed from `"auto"/"none"` to `"Auto"/"None"` per schema. Updated 4 test assertions in test_schema_conformance.py and test_serialization.py.
6. **ColumnBuilder.with_width()** — widened type hint to `str | int | float` (file already uses `from __future__ import annotations`).
7. **BlockElementHeight enum** — added to enums.py, exported from `__init__.py`.

Key pattern learned: `with_requires` consistently uses `(key: str, version: str)` across all 18+ builders — not `(requires: dict)`. Always follow existing codebase patterns over task descriptions.

### 2025-07-23 — Add to_dict() for Native Object Serialization (#75)

- **New function**: `to_dict(card)` in `serialization.py` — applies same cleanup as `to_json()` (strips None, converts enums to plain strings) but returns a `dict` instead of JSON string. Avoids double-serialization when embedding in API responses.
- **Implementation**: Added `_clean()` helper alongside existing `_strip_none()`. `_clean` also handles `Enum` → `str` coercion via `obj.value`.
- **Key insight**: Python `str, Enum` members survive in raw dicts as enum instances. `json.dumps` implicitly converts them, but `to_dict` must do it explicitly for true parity with `json.loads(to_json(card))`.
- **Exported**: Added `to_dict` to `__init__.py` imports and `__all__`.
- **Tests**: 7 new tests in `TestToDict` class, including equivalence test against `json.loads(to_json(card))`.
- **Sample**: Updated `program.py` with `to_dict` demo section.

### 2026-04-15 — Native Object Serialization (#75) — Cross-Team Coordination

Collaborated with McManus (.NET), Fenster (TypeScript), and Verbal (Tester) on Issue #75 implementation. Python `to_dict()` function complete with 8 new tests (371 total, was 363). All three core ports now provide equivalent native object methods with identical cleanup semantics: .NET `ToJsonElement()`/`ToJsonNode()`, TypeScript `toObject()`, Python `to_dict()`. Cross-port equivalence tests written by Verbal validate that all implementations produce identical output to `JSON.parse(toJson())` / `json.loads(to_json())`. Go port deferred pending architecture decision.
