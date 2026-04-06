# FluentCards — Node.js / TypeScript

A TypeScript library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and built-in validation.

## Installation

```bash
npm install fluent-cards
```

## Quick Start

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

## Project Layout

```
node/
├── fluent-cards/          # Library package
│   ├── src/
│   │   ├── builders/      # Fluent builder classes
│   │   ├── enums.ts       # String enums
│   │   ├── models.ts      # Interfaces & discriminated unions
│   │   ├── serialization.ts
│   │   ├── validation.ts
│   │   └── index.ts       # Barrel export
│   ├── tsconfig.json
│   └── package.json
├── fluent-cards-tests/    # Test suite (node:test + tsx)
│   ├── test/
│   ├── tsconfig.json
│   └── package.json
└── package.json           # npm workspace root
```

## Build & Test

```bash
cd node
npm install
npm test
npm run typecheck
```

See the [root README](https://github.com/rido-min/FluentCards#readme) for more details.
