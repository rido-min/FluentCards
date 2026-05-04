# FluentCards — Node.js / TypeScript

A TypeScript library for building [Adaptive Cards](https://adaptivecards.io/) using a fluent builder pattern with strong typing and built-in validation.

**Supported runtimes:**
- Node.js 20+ (npm package: `fluent-cards`)
- Deno 2+ (JSR package: `jsr:@adaptivecards/fluent`)

## Installation

### Node.js (npm)

```bash
npm install fluent-cards
```

### Deno (JSR)

```bash
deno add jsr:@adaptivecards/fluent
```

## Quick Start

### Node.js

```typescript
import { AdaptiveCardBuilder, TextSize, TextWeight, toJson } from 'fluent-cards';

const card = AdaptiveCardBuilder.create()
  .withVersion('1.5')
  .addTextBlock(tb => tb
    .withText('Hello, FluentCards!')
    .withSize(TextSize.Large)
    .withWeight(TextWeight.Bolder)
    .withWrap(true))
  .addAction(a => a
    .openUrl('https://adaptivecards.io')
    .withTitle('Learn More'))
  .build();

console.log(toJson(card));
```

### Deno

```typescript
import { AdaptiveCardBuilder, TextSize, TextWeight, toJson } from 'jsr:@adaptivecards/fluent';

const card = AdaptiveCardBuilder.create()
  .withVersion('1.5')
  .addTextBlock(tb => tb
    .withText('Hello, FluentCards!')
    .withSize(TextSize.Large)
    .withWeight(TextWeight.Bolder)
    .withWrap(true))
  .addAction(a => a
    .openUrl('https://adaptivecards.io')
    .withTitle('Learn More'))
  .build();

console.log(toJson(card));
```

## Samples

- **Node.js samples:** [`node/samples/`](../../samples/)
- **Deno samples:** [`node/samples/deno/`](../../samples/deno/)

Run Node samples:
```bash
cd node/samples
node --require tsx/cjs program.ts
```

Run Deno samples:
```bash
cd node/samples/deno
deno run program.ts
```

## Deno Tests

The Deno test suite validates the JSR-published library in Deno runtime. Tests are located in `tests-deno/` and use Deno's built-in test runner.

**Run Deno tests:**
```bash
cd node/packages/fluent-cards
deno test tests-deno/ --sloppy-imports
```

**Note:** `--sloppy-imports` is required because the library source uses `.js` extensions in imports (for Node.js compatibility), but Deno resolves them to `.ts` at runtime.

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

## API Overview

All elements use the builder pattern: `create()` → `withX()` / `addX(configure)` → `build()`.

Available builders: `AdaptiveCardBuilder`, `TextBlockBuilder`, `ImageBuilder`, `ContainerBuilder`, `ColumnSetBuilder`, `ColumnBuilder`, `FactSetBuilder`, `RichTextBlockBuilder`, `TextRunBuilder`, `ActionSetBuilder`, `MediaBuilder`, `ImageSetBuilder`, `TableBuilder`, `ActionBuilder`, `BackgroundImageBuilder`, `RefreshBuilder`, `AuthenticationBuilder`, and input builders (`InputTextBuilder`, `InputNumberBuilder`, `InputDateBuilder`, `InputTimeBuilder`, `InputToggleBuilder`, `InputChoiceSetBuilder`).

## Build & Test

```bash
cd node
npm install
npm test
npm run typecheck
```

See the [root README](https://github.com/rido-min/FluentCards#readme) for more details.
