# FluentCards

[![CI](https://github.com/rido-min/FluentCards/actions/workflows/ci.yml/badge.svg)](https://github.com/rido-min/FluentCards/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/FluentCards.svg)](https://www.nuget.org/packages/FluentCards)
[![npm](https://img.shields.io/badge/npm-fluent--cards-blue)](https://www.npmjs.com/package/fluent-cards)
[![PyPI](https://img.shields.io/pypi/v/fluent-cards.svg)](https://pypi.org/project/fluent-cards/)
[![Go Reference](https://pkg.go.dev/badge/github.com/rido-min/FluentCards/go/fluentcards.svg)](https://pkg.go.dev/github.com/rido-min/FluentCards/go/fluentcards)

A multi-language library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and schema validation.

## Language Ports

| Language | Folder | Package |
|----------|--------|---------|
| C# / .NET 8 | [`dotnet/`](dotnet/) | [![NuGet](https://img.shields.io/nuget/v/FluentCards.svg)](https://www.nuget.org/packages/FluentCards) |
| TypeScript / Node.js | [`node/`](node/) | [![npm](https://img.shields.io/badge/npm-fluent--cards-blue)](https://www.npmjs.com/package/fluent-cards) |
| Python | [`python/`](python/) | [![PyPI](https://img.shields.io/pypi/v/fluent-cards.svg)](https://pypi.org/project/fluent-cards/) |
| Go | [`go/`](go/) | [![Go Reference](https://pkg.go.dev/badge/github.com/rido-min/FluentCards/go/fluentcards.svg)](https://pkg.go.dev/github.com/rido-min/FluentCards/go/fluentcards) |

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
from fluent_cards import AdaptiveCardBuilder, TextSize, TextWeight, to_json

card = (AdaptiveCardBuilder.create()
    .with_version('1.5')
    .add_text_block(lambda tb: tb
        .with_text('Hello, FluentCards!')
        .with_size(TextSize.Large)
        .with_weight(TextWeight.Bolder)
        .with_wrap(True))
    .build())

print(to_json(card))
```

### Go

```bash
go get github.com/rido-min/FluentCards/go
```

```go
import "github.com/rido-min/FluentCards/go/fluentcards"

card := fluentcards.NewAdaptiveCardBuilder().
    WithVersion("1.5").
    AddTextBlock(func(tb *fluentcards.TextBlockBuilder) {
        tb.WithText("Hello, FluentCards!").
            WithSize(fluentcards.TextSizeLarge).
            WithWeight(fluentcards.TextWeightBolder).
            WithWrap(true)
    }).
    Build()

json, _ := fluentcards.ToJSON(card)
fmt.Println(json)
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
