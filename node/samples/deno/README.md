# FluentCards Deno Samples

This directory contains Deno samples for the FluentCards library.

## Prerequisites

- [Deno](https://deno.com/) 2.0 or later

## Running Samples

Each sample can be run individually or via the main program:

```bash
# Run the main program (all samples)
deno run program.ts

# Run individual samples
deno run basic_card_sample.ts
deno run form_card_sample.ts
deno run layout_card_sample.ts
deno run people_picker_sample.ts
deno run rich_content_sample.ts
deno run action_submit_execute_sample.ts
deno run validation_sample.ts
```

## Import Strategy

Samples use JSR imports unconditionally:

```typescript
import { AdaptiveCardBuilder } from 'jsr:@adaptivecards/fluent';
```

### Local Development (Before First Publish)

An import map (`deno.json` in this directory) redirects the JSR import to the local source and enables sloppy-imports:

```json
{
  "unstable": ["sloppy-imports"],
  "imports": {
    "jsr:@adaptivecards/fluent": "../../packages/fluent-cards/src/index.ts"
  }
}
```

This means:
- **No code changes needed** — samples work identically in local dev and after JSR publish
- **Before publish:** Deno uses the local source via the import map
- **After publish:** You can delete the import map (or just the `imports` field) and Deno will fetch from JSR

The `sloppy-imports` setting allows Deno to resolve `.js` extensions (required by npm's CommonJS build) to `.ts` files at runtime.

This eliminates the import-swap dance present in earlier versions.

## Sample Overview

| File | Description |
|------|-------------|
| `basic_card_sample.ts` | Simple cards with text, images, and actions |
| `form_card_sample.ts` | Input elements (text, number, date, toggle, choice set) |
| `layout_card_sample.ts` | ColumnSet, Container, FactSet layouts |
| `people_picker_sample.ts` | People picker via `withChoicesData` |
| `rich_content_sample.ts` | RichTextBlock, ImageSet, Media, Table |
| `action_submit_execute_sample.ts` | Action.Execute and Action.Submit with custom verbs/data |
| `validation_sample.ts` | `validate()`, `validateAndThrow()`, error handling |
| `program.ts` | Entry point that runs all samples |

## Permissions

Most samples require no permissions. If a sample needs network access (e.g., future fetch examples), run with:

```bash
deno run --allow-net <sample_file>.ts
```

## Notes

- Samples follow Deno's snake_case file naming convention
- All samples use the `import.meta.main` pattern so they can be run directly or imported as modules
- Type imports use the `import type` syntax for better tree-shaking
