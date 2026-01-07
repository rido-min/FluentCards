# FluentCards

A .NET library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and JSON serialization powered by Source Generators.

## Features

- ✨ **Fluent Builder Pattern**: Intuitive and readable API for creating Adaptive Cards
- 🔒 **Strong Typing**: Full type safety with IntelliSense support
- ⚡ **Source Generators**: High-performance JSON serialization using `System.Text.Json` source generators
- 🎯 **AOT Compatible**: Ready for Native AOT compilation
- 📦 **Minimal Dependencies**: Built on .NET 8.0 with no external dependencies

## Installation

```bash
dotnet add package FluentCards
```

## Quick Start

```csharp
using FluentCards;

var card = AdaptiveCardBuilder.Create()
    .WithVersion("1.5")
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

### Output

```json
{
  "type": "AdaptiveCard",
  "version": "1.5",
  "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
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
    .WithVersion("1.5")
    .WithSchema("http://adaptivecards.io/schemas/adaptive-card.json")
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

## Supported Elements

### Current Support (Phase 1)

- ✅ **AdaptiveCard**: Root card container
- ✅ **TextBlock**: Rich text display with formatting
- ✅ **OpenUrlAction**: Open URL actions

### Enums

- **TextSize**: `Small`, `Default`, `Medium`, `Large`, `ExtraLarge`
- **TextWeight**: `Lighter`, `Default`, `Bolder`
- **TextColor**: `Default`, `Dark`, `Light`, `Accent`, `Good`, `Attention`, `Warning`
- **HorizontalAlignment**: `Left`, `Center`, `Right`

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

## Roadmap

Future phases will include:

- Additional card elements (Image, Container, ColumnSet, etc.)
- More action types (Submit, ShowCard, ToggleVisibility)
- Input elements (Text, Number, Date, Choice, Toggle)
- Advanced features (Fallback, Refresh, Authentication)

## References

- [Adaptive Cards Documentation](https://adaptivecards.io/)
- [Adaptive Cards Schema Explorer](https://adaptivecards.io/explorer/)
- [System.Text.Json Source Generation](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation)
