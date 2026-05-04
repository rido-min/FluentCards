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

The Deno test suite validates the library in Deno runtime. Tests are located in `tests-deno/` and use Deno's built-in test runner.

**Run Deno tests:**
```bash
cd node/packages/fluent-cards
deno test tests-deno/
```

The workspace-root `node/deno.json` enables sloppy-imports, allowing Deno to resolve `.js` extensions (required by npm's CommonJS build) to `.ts` files at runtime.

## JSR Publishing

For JSR publication, the library source (`src/`) is consumed directly by JSR — no build step needed. The workspace-root `node/deno.json` enables sloppy-imports, which allows JSR to accept the `.js` extensions in TypeScript source that npm's CommonJS build requires.

**Validate JSR publication locally:**
```bash
cd node/packages/fluent-cards
deno publish --dry-run --allow-dirty
```

The `--allow-dirty` flag is needed because the codecov binary leaves the working tree dirty during CI. In local dev, you can omit it if your working tree is clean.

CI runs validation automatically on every push and publishes to JSR on tagged releases.

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
