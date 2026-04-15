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
