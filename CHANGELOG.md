# Changelog

All notable changes to FluentCards will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- `AdaptiveCardVersion` enum (`V1_0`–`V1_6`) with `ToVersionString()`, `ToSchemaUrl()`, and `TryParse()` for type-safe schema version selection
- `WithVersion(AdaptiveCardVersion)` builder overload with automatic `$schema` URL synchronization
- `VersionInfo` static class mapping all element types, action types, and card properties to their introduction version
- `VERSION_MISMATCH` validation warnings when card uses features newer than declared version
- Multi-version schema conformance tests (1.2–1.6)
- `<remarks>Added in Adaptive Cards X.Y.</remarks>` XML doc annotations on all post-1.0 types and properties
- Initial release of FluentCards library
- Fluent builder API for creating Adaptive Cards
- Full Adaptive Cards 1.6.0 schema support
- AOT-compatible JSON serialization using System.Text.Json source generators
- Comprehensive validation system with structured error reporting
- Test-time JSON schema validation against the official 1.6.0 schema
- XML documentation for IntelliSense support
- Sample code demonstrating common patterns

### Changed
- `WithVersion(string)` now auto-sets `$schema` URL for recognized version strings
- `AdaptiveCardValidator.IsValidVersion()` replaced with `AdaptiveCardVersionExtensions.TryParse()`

### Fixed
- `ChoiceInputStyle.Filtered` version annotation corrected from 1.5 to 1.6 (per official schema)
- `selectAction` version corrected from 1.0 to 1.1
- `minHeight` and `verticalContentAlignment` versions corrected from 1.0 to 1.2

### Schema 1.6.0 Coverage
- Added `FontType`, `TextBlockStyle`, `InputLabelPosition`, `InputStyle`, `ActionMode`, typed `Spacing` enums
- Added `CaptionSource` and `DataQuery` types
- Added missing properties to `AdaptiveCard` (selectAction, fallbackText, minHeight, rtl, speak, lang, verticalContentAlignment)
- Added missing properties to `TextBlock` (fontType, isSubtle, style)
- Added missing properties to `AdaptiveAction` (fallback, mode)
- Added missing properties to `InputElement` (labelPosition, labelWidth, inputStyle)
- Added missing properties to `Media` (captionSources)
- Added missing properties to `Column` (fallback, height, separator, spacing, isVisible, requires, rtl)
- Added `fontType` property to `TextRun`

### Validation Rules
- Required property checks: Container.Items, FactSet.Facts, Media.Sources, ImageSet.Images, RichTextBlock.Inlines, ActionSet.Actions, Input.Toggle.Title, ToggleVisibilityAction.TargetElements, TextBlock.Text
- Relationship validation: min ≤ max for InputNumber/Date/Time
- SelectAction type restriction: ShowCard not allowed as selectAction
- Duplicate element ID detection
- Deep nested validation for Table, ActionSet, ImageSet, Container, ColumnSet

### Supported Elements
- **Text**: TextBlock, RichTextBlock
- **Containers**: Container, ColumnSet, Column
- **Inputs**: Input.Text, Input.Number, Input.Date, Input.Time, Input.Toggle, Input.ChoiceSet
- **Media**: Image, ImageSet, Media
- **Data**: FactSet, Table, ActionSet
- **Actions**: Action.OpenUrl, Action.Submit, Action.ShowCard, Action.ToggleVisibility, Action.Execute

### Features
- Fluent builder pattern with type safety
- Polymorphic serialization for elements and actions
- Custom JSON converters for union types
- Validation with structured error reporting
- Refresh and authentication configuration support
- Fallback element support

## [1.0.0] - TBD

Initial public release.
