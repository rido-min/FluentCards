# TypeScript Schema Conformance Fixes

**Author:** Fenster (TypeScript Dev)
**Date:** 2025-07-22
**Related:** keaton-ts-schema-audit.md

## Summary

Fixed all 8 conformance gaps + 1 enum casing issue from Keaton's TS schema audit. All 247 tests pass, library typechecks clean.

## Decisions Made

### AssociatedInputs schema casing
Kept `AssociatedInputs` JSON values as `'auto'` and `'none'` to match Adaptive Cards payload casing and cross-port behavior.

### Column width `string | number`
`Column.width` and `TableColumnDefinition.width` now accept `string | number`. The schema allows numeric values as relative column weights. `ColumnBuilder.withWidth()` and `ColumnSetBuilder.addColumn()` width parameters updated to match.

### TextBlockBuilder base element methods
Added `withHeight()`, `withFallback()`, `withRequires()` to `TextBlockBuilder` following the same pattern used by `ContainerBuilder`, `MediaBuilder`, `ColumnBuilder`, etc. Did **not** add `withRtl()` — per audit Design Note B, `rtl` on TextBlock is over-broad (schema only defines it on Container/Column/TableCell/AdaptiveCard). The `rtl` field is available via `AdaptiveElementBase` but we don't surface it in the builder to avoid encouraging non-spec usage.

## Cross-Port Note
`AssociatedInputs` remains lowercase (`'auto'`/`'none'`) for parity with .NET and existing host expectations.
