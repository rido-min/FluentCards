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

## Build & Test

```bash
cd dotnet
dotnet build --configuration Release
dotnet test --configuration Release --no-build
```

See the [root README](../README.md) for more details and the full feature list.
