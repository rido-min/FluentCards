# FluentCards

[![CI](https://github.com/rido-min/FluentCards/actions/workflows/ci.yml/badge.svg)](https://github.com/rido-min/FluentCards/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/FluentCards.svg)](https://www.nuget.org/packages/FluentCards)
[![NuGet Downloads](https://img.shields.io/nuget/dt/FluentCards.svg)](https://www.nuget.org/packages/FluentCards)

A multi-language library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and schema validation.

## Language Ports

| Language | Folder | Package |
|----------|--------|---------|
| C# / .NET 8 | [`dotnet/`](dotnet/) | [![NuGet](https://img.shields.io/nuget/v/FluentCards.svg)](https://www.nuget.org/packages/FluentCards) |
| TypeScript / Node.js | [`node/`](node/) | npm (coming soon) |
| Python | [`python/`](python/) | PyPI (coming soon) |

## Quick Start

### C# / .NET

```bash
dotnet add package FluentCards
```

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

### TypeScript / Node.js

```bash
npm install fluent-cards
```

```typescript
import { AdaptiveCardBuilder, TextSize, TextWeight, toJson } from 'fluent-cards';

const card = AdaptiveCardBuilder.create()
  .withVersion('1.5')
  .addTextBlock(tb => tb
    .withText('Hello, FluentCards!')
    .withSize(TextSize.Large)
    .withWeight(TextWeight.Bolder)
    .withWrap(true))
  .build();

console.log(toJson(card));
```

### Python

```bash
pip install fluent-cards
```

```python
from fluent_cards import AdaptiveCardBuilder, TextSize, TextWeight

card = (AdaptiveCardBuilder.create()
    .with_version('1.5')
    .add_text_block(lambda tb: tb
        .with_text('Hello, FluentCards!')
        .with_size(TextSize.LARGE)
        .with_weight(TextWeight.BOLDER)
        .with_wrap(True))
    .build())

print(card.to_json())
```

## Documentation

- [Schema Validation](docs/schema-validation.md) — built-in validation, version-aware checks, and schema conformance testing
- [Teams Adaptive Cards](docs/teams-cards.md) — pre-built card layouts for Microsoft Teams notifications

## License

MIT — see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## References

- [Adaptive Cards Documentation](https://adaptivecards.io/)
- [Adaptive Cards Schema Explorer](https://adaptivecards.io/explorer/)
