# TypeScript Schema Conformance Fixes

**Author:** Fenster (TypeScript Dev)
**Date:** 2025-07-22
**Related:** keaton-ts-schema-audit.md

## Summary

Fixed all 8 conformance gaps + 1 enum casing issue from Keaton's TS schema audit. All 247 tests pass, library typechecks clean.

## Decisions Made

### AssociatedInputs PascalCase (Breaking Change)
Changed `AssociatedInputs.Auto` from `'auto'` to `'Auto'` and `AssociatedInputs.None` from `'none'` to `'None'`. This matches the canonical Adaptive Cards 1.6.0 schema values. **This is a behavioral change** — any downstream code comparing serialized values against `'auto'`/`'none'` will need updating. Updated all test assertions accordingly.

### Column width `string | number`
`Column.width` and `TableColumnDefinition.width` now accept `string | number`. The schema allows numeric values as relative column weights. `ColumnBuilder.withWidth()` and `ColumnSetBuilder.addColumn()` width parameters updated to match.

### TextBlockBuilder base element methods
Added `withHeight()`, `withFallback()`, `withRequires()` to `TextBlockBuilder` following the same pattern used by `ContainerBuilder`, `MediaBuilder`, `ColumnBuilder`, etc. Did **not** add `withRtl()` — per audit Design Note B, `rtl` on TextBlock is over-broad (schema only defines it on Container/Column/TableCell/AdaptiveCard). The `rtl` field is available via `AdaptiveElementBase` but we don't surface it in the builder to avoid encouraging non-spec usage.

## Cross-Port Note
The `AssociatedInputs` PascalCase change should be mirrored in Python and Go ports if their enum values are also lowercase. .NET already uses PascalCase.
