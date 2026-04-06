# Schema Validation

FluentCards includes built-in validation to ensure your cards meet the Adaptive Cards schema requirements.

## Basic Validation

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion("1.5")
    .AddTextBlock(tb => tb.WithText("Hello World"))
    .Build();

var issues = AdaptiveCardValidator.Validate(card);
if (issues.Count == 0)
{
    Console.WriteLine("Card is valid!");
}
else
{
    foreach (var issue in issues)
    {
        Console.WriteLine($"[{issue.Severity}] {issue.Path}: {issue.Message}");
    }
}
```

To throw on validation errors instead of returning a list:

```csharp
try
{
    AdaptiveCardValidator.ValidateAndThrow(card);
}
catch (AdaptiveCardValidationException ex)
{
    Console.WriteLine($"Validation failed: {ex.Message}");
    foreach (var error in ex.Errors)
    {
        Console.WriteLine($"  - [{error.Path}] {error.Message}");
    }
}
```

## What Is Validated

- Required properties (version, type, text, URLs, input IDs, toggle title, facts, actions, inlines, sources, images, target elements)
- Schema version compatibility
- Empty elements, containers, and cards
- URL format validation
- Nested element validation (Container, ColumnSet, Table, ActionSet, ImageSet)
- Action count limits
- Min/max consistency for numeric, date, and time inputs
- Duplicate element ID detection
- SelectAction type restrictions (ShowCard not allowed)
- Version mismatch detection (features requiring a newer schema version than declared)

## Version-Aware Validation

The validator detects when a card uses features that require a newer Adaptive Cards version than what is declared, emitting `VERSION_MISMATCH` warnings:

```csharp
// A card declaring version 1.2 but using Table (introduced in 1.5)
var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_2)
    .AddTable(t => t
        .AddColumn(c => c.WithWidth(1))
        .AddRow(r => r.AddCell(cell => cell
            .AddTextBlock(tb => tb.WithText("Data")))))
    .Build();

var issues = AdaptiveCardValidator.Validate(card);

foreach (var issue in issues)
{
    Console.WriteLine($"[{issue.Severity}] {issue.Code}: {issue.Message}");
    // [Warning] VERSION_MISMATCH: Table requires Adaptive Cards 1.5
    //   but card declares version 1.2
}
```

Version mismatch checks are warnings only — serialization is never affected. The `VersionInfo` class maps every element type, action type, and card property to its introduction version.

## Schema Conformance Testing

FluentCards validates generated JSON against the official Adaptive Cards JSON schemas. The test matrix covers schema versions 1.2, 1.3, 1.4, 1.5, and 1.6:

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_6)
    .AddTextBlock(tb => tb.WithText("Hello World"))
    .Build();

// Validates card JSON against the embedded schema for the declared version
SchemaValidator.AssertValid(card);
```

## Custom Schema URL

To override the auto-generated `$schema` URL:

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_5)
    .WithSchema("https://example.com/custom-schema.json")
    .AddTextBlock(tb => tb.WithText("Hello World"))
    .Build();
```
