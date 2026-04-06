# FluentCards Documentation

FluentCards is a multi-language library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and schema validation.

## Language Ports

| Language | Package | API Reference |
|---|---|---|
| C# / .NET 8 | [![NuGet](https://img.shields.io/nuget/v/FluentCards.svg)](https://www.nuget.org/packages/FluentCards) | .NET API Reference |
| TypeScript / Node.js | npm (coming soon) | [TypeScript API Reference](./api-node.md) |
| Python | PyPI (coming soon) | [Python API Reference](./api-python.md) |

## Quick Start

### C# / .NET

```bash
dotnet add package FluentCards
```

```csharp
using FluentCards;

var card = AdaptiveCardBuilder.Create()
    .WithVersion("1.5")
    .AddTextBlock(tb => tb
        .WithText("Hello World")
        .WithWeight(TextWeight.Bolder))
    .Build();
```

### TypeScript / Node.js

```bash
npm install fluent-cards
```

```typescript
import { AdaptiveCardBuilder, TextWeight } from 'fluent-cards';

const card = new AdaptiveCardBuilder()
  .withVersion('1.5')
  .addTextBlock(tb => tb
    .withText('Hello World')
    .withWeight(TextWeight.Bolder))
  .build();
```

### Python

```bash
pip install fluent-cards
```

```python
from fluent_cards import AdaptiveCardBuilder, TextWeight

card = (AdaptiveCardBuilder.create()
    .with_version('1.5')
    .add_text_block(lambda tb: tb
        .with_text('Hello World')
        .with_weight(TextWeight.Bolder))
    .build())
```

## Articles

- [Schema Validation](schema-validation.md)
- [Teams Adaptive Cards](teams-cards.md)
