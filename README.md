# FluentCards

A .NET library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and JSON serialization powered by Source Generators.

## Features

- ✨ **Fluent Builder Pattern**: Intuitive and readable API for creating Adaptive Cards
- 🔒 **Strong Typing**: Full type safety with IntelliSense support
- ⚡ **Source Generators**: High-performance JSON serialization using `System.Text.Json` source generators
- 🎯 **AOT Compatible**: Ready for Native AOT compilation
- 📦 **Minimal Dependencies**: Built on .NET 8.0 with no external dependencies
- ✅ **Built-in Validation**: Comprehensive validation for card structure and schema compliance
- 🏷️ **Version-Aware**: `AdaptiveCardVersion` enum for type-safe version selection, automatic `$schema` URL synchronization, and `VERSION_MISMATCH` validation warnings
- 📚 **Rich Samples**: Extensive examples demonstrating common patterns and best practices
- 📖 **Complete XML Documentation**: Full IntelliSense support with detailed API documentation

## Installation

```bash
dotnet add package FluentCards
```

## Quick Start

```csharp
using FluentCards;

var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_5)  // auto-sets $schema URL
    .AddTextBlock(tb => tb
        .WithText("Hello, FluentCards!")
        .WithSize(TextSize.Large)
        .WithWeight(TextWeight.Bolder)
        .WithWrap(true))
    .AddTextBlock(tb => tb
        .WithText("This card was built with a fluent interface.")
        .WithColor(TextColor.Accent))
    .Build();

Console.WriteLine(card.ToJson());
```

The string overload `WithVersion("1.5")` is also supported and auto-sets the `$schema` URL for recognized versions.

### Output

```json
{
  "type": "AdaptiveCard",
  "version": "1.5",
  "$schema": "https://adaptivecards.io/schemas/1.5.0/adaptive-card.json",
  "body": [
    {
      "type": "TextBlock",
      "text": "Hello, FluentCards!",
      "size": "large",
      "weight": "bolder",
      "wrap": true
    },
    {
      "type": "TextBlock",
      "text": "This card was built with a fluent interface.",
      "color": "accent"
    }
  ]
}
```

## Core Concepts

### Building Cards

Use the `AdaptiveCardBuilder` to create cards with a fluent API:

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_5)  // auto-sets $schema URL
    .AddTextBlock(tb => tb.WithText("Hello World"))
    .Build();
```

You can also use the string overload — it auto-sets the `$schema` URL for recognized versions:

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion("1.5")
    .AddTextBlock(tb => tb.WithText("Hello World"))
    .Build();
```

To set a custom schema URL, use `WithSchema()` which overrides the auto-generated URL:

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_5)
    .WithSchema("https://example.com/custom-schema.json")
    .AddTextBlock(tb => tb.WithText("Hello World"))
    .Build();
```

### Text Blocks

Create rich text elements with various formatting options:

```csharp
.AddTextBlock(tb => tb
    .WithId("myTextBlock")
    .WithText("Important Message")
    .WithSize(TextSize.Large)
    .WithWeight(TextWeight.Bolder)
    .WithColor(TextColor.Attention)
    .WithWrap(true)
    .WithMaxLines(3)
    .WithHorizontalAlignment(HorizontalAlignment.Center))
```

### Actions

Add interactive actions to your cards:

```csharp
var card = AdaptiveCardBuilder.Create()
    .AddAction(a => a
        .OpenUrl("https://adaptivecards.io")
        .WithTitle("Learn More"))
    .Build();
```

### Serialization

Convert cards to and from JSON:

```csharp
// Serialize to JSON
string json = card.ToJson();

// Deserialize from JSON
AdaptiveCard? card = AdaptiveCardExtensions.FromJson(json);
```

### Validation

FluentCards includes built-in validation to ensure your cards meet the Adaptive Cards schema requirements:

```csharp
// Validate a card
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

// Validate and throw on errors
try
{
    AdaptiveCardValidator.ValidateAndThrow(card);
    // Card is valid
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

Validation checks include:
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

### Version-Aware Validation

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

var validator = new AdaptiveCardValidator();
var issues = validator.Validate(card);

foreach (var issue in issues)
{
    Console.WriteLine($"[{issue.Severity}] {issue.Code}: {issue.Message}");
    // [Warning] VERSION_MISMATCH: Table requires Adaptive Cards 1.5
    //   but card declares version 1.2
}
```

Version mismatch checks are warnings only — serialization is never affected. The `VersionInfo` class maps every element type, action type, and card property to its introduction version.

### Schema Conformance Testing

FluentCards includes test-time validation against the official Adaptive Cards JSON schemas, ensuring generated JSON always conforms to the spec. The test matrix validates against schemas for versions 1.2, 1.3, 1.4, 1.5, and 1.6:

```csharp
// In your test project, add the FluentCards.Tests package or reference the SchemaValidator helper
var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_6)
    .AddTextBlock(tb => tb.WithText("Hello World"))
    .Build();

// Validates card JSON against the embedded schema for the declared version
SchemaValidator.AssertValid(card);
```

### Input Forms

Create interactive forms with validation:

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion("1.5")
    .AddTextBlock(tb => tb
        .WithText("Contact Form")
        .WithSize(TextSize.Large)
        .WithWeight(TextWeight.Bolder))
    .AddInputText(i => i
        .WithId("name")
        .WithLabel("Name")
        .WithPlaceholder("Enter your name")
        .IsRequired(true)
        .WithErrorMessage("Name is required"))
    .AddInputText(i => i
        .WithId("email")
        .WithLabel("Email")
        .WithStyle(TextInputStyle.Email)
        .IsRequired(true))
    .AddInputText(i => i
        .WithId("message")
        .WithLabel("Message")
        .IsMultiline(true)
        .WithMaxLength(500))
    .AddAction(a => a
        .Submit("Send")
        .WithStyle(ActionStyle.Positive))
    .Build();
```

### Layouts

Create complex layouts with containers and columns:

```csharp
var card = AdaptiveCardBuilder.Create()
    .WithVersion("1.5")
    .AddColumnSet(cs => cs
        .AddColumn(col => col
            .WithWidth("auto")
            .AddImage(img => img
                .WithUrl("https://example.com/image.png")
                .WithSize(ImageSize.Medium)))
        .AddColumn(col => col
            .WithWidth("stretch")
            .AddTextBlock(tb => tb
                .WithText("Product Name")
                .WithWeight(TextWeight.Bolder))
            .AddTextBlock(tb => tb
                .WithText("Product description goes here")
                .WithWrap(true))))
    .AddAction(a => a
        .OpenUrl("https://example.com")
        .WithTitle("Learn More"))
    .Build();
```

## Samples

The library includes comprehensive samples demonstrating common patterns:

- **BasicCardSample**: Simple cards with text, images, and actions
- **FormCardSample**: Interactive forms with various input types
- **LayoutCardSample**: Advanced layouts using containers, columns, and fact sets
- **RichContentSample**: Rich content including tables, media, and formatted text

View the sample code in the [samples/FluentCards.Samples](samples/FluentCards.Samples) directory.

## Supported Elements

FluentCards supports the full Adaptive Cards 1.6.0 schema:

### Display Elements
- ✅ **TextBlock**: Rich text display with formatting
- ✅ **Image**: Display images from URLs
- ✅ **Container**: Group elements together
- ✅ **ColumnSet** and **Column**: Multi-column layouts
- ✅ **FactSet**: Display name-value pairs
- ✅ **RichTextBlock**: Mixed inline formatting with TextRun
- ✅ **ImageSet**: Display multiple images in a gallery
- ✅ **Table**: Tabular data display
- ✅ **Media**: Audio and video playback
- ✅ **ActionSet**: Group actions inline

### Input Elements
- ✅ **Input.Text**: Single or multi-line text input
- ✅ **Input.Number**: Numeric input
- ✅ **Input.Date**: Date picker
- ✅ **Input.Time**: Time picker
- ✅ **Input.Toggle**: Checkbox/toggle
- ✅ **Input.ChoiceSet**: Radio buttons or dropdown

### Actions
- ✅ **Action.OpenUrl**: Open a URL
- ✅ **Action.Submit**: Submit form data
- ✅ **Action.ShowCard**: Show a nested card
- ✅ **Action.ToggleVisibility**: Show/hide elements
- ✅ **Action.Execute**: Execute a command

### Advanced Features
- ✅ **Refresh**: Auto-refresh configuration
- ✅ **Authentication**: SSO authentication
- ✅ **Fallback**: Graceful degradation
- ✅ **Requires**: Version requirements
- ✅ **Metadata**: Card metadata

### Enums

- **TextSize**: `Small`, `Default`, `Medium`, `Large`, `ExtraLarge`
- **TextWeight**: `Lighter`, `Default`, `Bolder`
- **TextColor**: `Default`, `Dark`, `Light`, `Accent`, `Good`, `Attention`, `Warning`
- **FontType**: `Default`, `Monospace`
- **TextBlockStyle**: `Default`, `Heading`
- **HorizontalAlignment**: `Left`, `Center`, `Right`
- **VerticalAlignment**: `Top`, `Center`, `Bottom`
- **ContainerStyle**: `Default`, `Emphasis`, `Good`, `Attention`, `Warning`, `Accent`
- **ImageSize**: `Auto`, `Stretch`, `Small`, `Medium`, `Large`
- **ActionStyle**: `Default`, `Positive`, `Destructive`
- **ActionMode**: `Primary`, `Secondary`
- **Spacing**: `Default`, `None`, `Small`, `Medium`, `Large`, `ExtraLarge`, `Padding`
- **InputLabelPosition**: `Inline`, `Above`
- **InputStyle**: `Default`, `RevealOnHover`

## Project Structure

```
FluentCards/
├── src/
│   └── FluentCards/           # Main library
├── tests/
│   └── FluentCards.Tests/     # Unit tests
└── samples/
    └── FluentCards.Samples/   # Usage examples
```

## Building from Source

```bash
# Clone the repository
git clone https://github.com/rido-min/FluentCards.git
cd FluentCards

# Build the solution
dotnet build

# Run tests
dotnet test

# Run samples
dotnet run --project samples/FluentCards.Samples
```

## Requirements

- .NET 8.0 or later

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## References

- [Adaptive Cards Documentation](https://adaptivecards.io/)
- [Adaptive Cards Schema Explorer](https://adaptivecards.io/explorer/)
- [System.Text.Json Source Generation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation)
