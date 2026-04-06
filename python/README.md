# FluentCards — Python

[![CI](https://github.com/rido-min/FluentCards/actions/workflows/ci.yml/badge.svg)](https://github.com/rido-min/FluentCards/actions/workflows/ci.yml)
[![PyPI](https://img.shields.io/pypi/v/fluent-cards.svg)](https://pypi.org/project/fluent-cards/)
[![Python](https://img.shields.io/pypi/pyversions/fluent-cards.svg)](https://pypi.org/project/fluent-cards/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A Python library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and built-in schema validation.

> Also available for [.NET (NuGet)](https://www.nuget.org/packages/FluentCards), [Node.js (npm)](https://www.npmjs.com/package/fluent-cards), and [Go](https://github.com/rido-min/FluentCards/tree/main/go).

## Installation

```bash
pip install fluent-cards
```

## Quick Start

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

Output:

```json
{
  "type": "AdaptiveCard",
  "version": "1.5",
  "body": [
    {
      "type": "TextBlock",
      "text": "Hello, FluentCards!",
      "size": "Large",
      "weight": "Bolder",
      "wrap": true
    }
  ]
}
```

## Features

- **Fluent builder API** — chainable, readable card construction
- **Strong typing** — enums for all Adaptive Cards schema values (`TextSize`, `TextWeight`, `Colors`, `ContainerStyle`, etc.)
- **Built-in validation** — catches schema errors before the card is sent
- **Zero dependencies** — pure Python, no third-party packages required
- **Full schema support** — TextBlock, Image, ColumnSet, Container, FactSet, ActionSet, and more

## More Examples

### Card with image and actions

```python
from fluent_cards import AdaptiveCardBuilder, TextSize, TextWeight, ActionStyle

card = (AdaptiveCardBuilder.create()
    .with_version('1.5')
    .add_text_block(lambda tb: tb
        .with_text('Order Confirmed')
        .with_size(TextSize.Large)
        .with_weight(TextWeight.Bolder))
    .add_text_block(lambda tb: tb
        .with_text('Your order #12345 has been placed.')
        .with_wrap(True))
    .add_action(lambda a: a
        .open_url('https://example.com/track/12345')
        .with_title('Track Order'))
    .build())
```

### Card with columns

```python
from fluent_cards import AdaptiveCardBuilder

card = (AdaptiveCardBuilder.create()
    .with_version('1.5')
    .add_column_set(lambda cs: cs
        .add_column(lambda col: col
            .with_width('auto')
            .add_text_block(lambda tb: tb.with_text('Name:')))
        .add_column(lambda col: col
            .with_width('stretch')
            .add_text_block(lambda tb: tb.with_text('Jane Doe'))))
    .build())
```

### Using choices (Input.ChoiceSet)

```python
from fluent_cards import AdaptiveCardBuilder

card = (AdaptiveCardBuilder.create()
    .with_version('1.5')
    .add_input_choice_set(lambda cs: cs
        .with_id('priority')
        .with_label('Priority')
        .add_choice('High', 'high')
        .add_choice('Medium', 'medium')
        .add_choice('Low', 'low'))
    .build())
```

### Validation

```python
from fluent_cards import AdaptiveCardBuilder, validate

card = AdaptiveCardBuilder.create().build()  # no version set
errors = validate(card)

for error in errors:
    print(error)
```

## API Reference

See the [full API docs](https://rido-min.github.io/FluentCards/python/) for all builder methods and enums.

## Development

```bash
git clone https://github.com/rido-min/FluentCards.git
cd FluentCards/python
pip install -e ".[dev]"
pytest
```

## License

MIT — see [LICENSE](https://github.com/rido-min/FluentCards/blob/main/LICENSE) for details.
