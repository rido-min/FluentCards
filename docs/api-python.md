# Python API Reference

The Python API reference is generated using [pdoc](https://pdoc.dev/).

## Generating the Docs

From the `python/` directory, run:

```bash
pip install pdoc
pdoc fluent_cards --output-dir ../_site/api/python
```

The generated HTML documentation will be output to `_site/api/python/`.

## Key Modules

- **[fluent_cards.builders.adaptive_card_builder](https://github.com/rido-min/FluentCards/tree/main/python/fluent_cards/builders/adaptive_card_builder.py)** — Entry point for building cards.
- **[fluent_cards.builders](https://github.com/rido-min/FluentCards/tree/main/python/fluent_cards/builders/)** — Per-element fluent builders.
- **[fluent_cards.models](https://github.com/rido-min/FluentCards/tree/main/python/fluent_cards/models.py)** — TypedDict definitions for all Adaptive Card elements.
- **[fluent_cards.enums](https://github.com/rido-min/FluentCards/tree/main/python/fluent_cards/enums.py)** — Enums for all schema-enumerated values.
- **[fluent_cards.validation](https://github.com/rido-min/FluentCards/tree/main/python/fluent_cards/validation.py)** — `validate()` and `validate_and_throw()`.
- **[fluent_cards.serialization](https://github.com/rido-min/FluentCards/tree/main/python/fluent_cards/serialization.py)** — `to_json()` and `from_json()`.
