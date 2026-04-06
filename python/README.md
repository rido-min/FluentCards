# FluentCards — Python

A Python library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and built-in validation.

## Installation

```bash
pip install fluent-cards
```

## Quick Start

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

## Project Layout

```
python/
├── fluent_cards/          # Library package
│   ├── builders/          # Fluent builder classes
│   ├── enums.py           # String enums
│   ├── models.py          # Dataclasses and types
│   ├── serialization.py
│   ├── validation.py
│   └── __init__.py        # Public API
└── tests/                 # pytest test suite
```

## Build & Test

```bash
cd python
pip install -e ".[dev]"
pytest
```

See the [root README](../README.md) for more details.
