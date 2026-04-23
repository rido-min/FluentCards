# FluentCards — .NET

A .NET 8 library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and AOT-compatible JSON serialization.

## Installation

```bash
dotnet add package FluentCards
```

## Quick Start

```csharp
using FluentCards;

var card = AdaptiveCardBuilder.Create()
    .WithVersion(AdaptiveCardVersion.V1_5)
    .AddTextBlock(tb => tb
        .WithText("Hello, FluentCards!")
        .WithSize(TextSize.Large)
        .WithWeight(TextWeight.Bolder)
        .WithWrap(true))
    .AddAction(a => a
        .OpenUrl("https://adaptivecards.io")
        .WithTitle("Learn More"))
    .Build();

Console.WriteLine(card.ToJson());
```

## Project Layout

```
dotnet/
├── src/FluentCards/          # Library source
├── tests/FluentCards.Tests/  # xUnit tests
├── samples/                  # Usage examples
├── benchmarks/               # BenchmarkDotNet benchmarks
└── scripts/                  # Screenshot and setup utilities
```

## API Overview

All elements use the builder pattern: `Create()` → `WithX()` / `AddX(configure)` → `Build()`.

Available builders: `AdaptiveCardBuilder`, `TextBlockBuilder`, `ImageBuilder`, `ContainerBuilder`, `ColumnSetBuilder`, `ColumnBuilder`, `FactSetBuilder`, `RichTextBlockBuilder`, `TextRunBuilder`, `ActionSetBuilder`, `MediaBuilder`, `ImageSetBuilder`, `TableBuilder`, `ActionBuilder`, `BackgroundImageBuilder`, `RefreshBuilder`, `AuthenticationBuilder`, and input builders (`InputTextBuilder`, `InputNumberBuilder`, `InputDateBuilder`, `InputTimeBuilder`, `InputToggleBuilder`, `InputChoiceSetBuilder`).

## Build & Test

```bash
cd dotnet
dotnet build --configuration Release
dotnet test --configuration Release --no-build
```

See the [root README](https://github.com/rido-min/FluentCards#readme) for all language ports, and [Schema Validation](../docs/schema-validation.md) and [Teams Cards](../docs/teams-cards.md) for more details.
