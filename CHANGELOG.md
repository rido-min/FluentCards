# Changelog

All notable changes to FluentCards will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Initial release of FluentCards library
- Fluent builder API for creating Adaptive Cards
- Full Adaptive Cards 1.5 schema support
- AOT-compatible JSON serialization using System.Text.Json source generators
- Comprehensive validation system
- XML documentation for IntelliSense support
- Sample code demonstrating common patterns

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
