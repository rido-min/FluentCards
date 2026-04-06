# TypeScript API Reference

The TypeScript API reference is generated using [TypeDoc](https://typedoc.org/).

## Generating the Docs

From the `node/packages/fluent-cards/` directory, run:

```bash
npm install
npm run docs
```

The generated HTML documentation will be output to `_site/api/node/`.

## Key Modules

- **[AdaptiveCardBuilder](https://github.com/rido-min/FluentCards/tree/main/node/packages/fluent-cards/src/builders/AdaptiveCardBuilder.ts)** — Entry point for building cards.
- **[Builders](https://github.com/rido-min/FluentCards/tree/main/node/packages/fluent-cards/src/builders/)** — Per-element fluent builders.
- **[Models](https://github.com/rido-min/FluentCards/tree/main/node/packages/fluent-cards/src/models.ts)** — TypeScript interfaces for all Adaptive Card elements.
- **[Enums](https://github.com/rido-min/FluentCards/tree/main/node/packages/fluent-cards/src/enums.ts)** — String enums for all schema-enumerated values.
- **[Validation](https://github.com/rido-min/FluentCards/tree/main/node/packages/fluent-cards/src/validation.ts)** — `validate()` and `validateAndThrow()`.
- **[Serialization](https://github.com/rido-min/FluentCards/tree/main/node/packages/fluent-cards/src/serialization.ts)** — `toJson()` and `fromJson()`.
