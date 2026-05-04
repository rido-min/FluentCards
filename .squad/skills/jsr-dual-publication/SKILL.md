# Skill: JSR Dual Publication for Pure-TypeScript Libraries

## When to Use This Skill

Use this pattern when:
- You have a **pure TypeScript library** with zero Node.js-specific APIs (`fs`, `process`, `Buffer`, `node:*` imports)
- You want to publish to **both npm and JSR** (JavaScript Registry for Deno/Bun/Cloudflare Workers)
- You want to **avoid code duplication** and maintain a single source of truth
- Your library is schema-driven or has strong typing requirements

Do **not** use this pattern when:
- Your library uses Node.js-specific APIs (use a compatibility layer or separate Deno port)
- Your library is browser-only (JSR is for server/runtime environments)
- You need separate release cadences for npm and JSR (though rare)

---

## Pattern Overview

**Goal:** Publish the same TypeScript library to both npm (as CommonJS or ESM) and JSR (as ESM with native TypeScript) from a single codebase.

**Key Principle:** JSR expects a **single source of truth** — it compiles TypeScript on-the-fly, eliminating the need for separate `.d.ts` files or compiled JS. This aligns with maintaining one codebase for both runtimes.

**Precedent:** Zod, Effect, tRPC, and many modern TS libraries use this pattern.

---

## Checklist

### 1. Verify Pure TypeScript (Critical)

Run this grep to detect Node.js-specific APIs:

```bash
grep -r "require\(|import.*from ['\"]\(node:\|fs\|path\|http\|util\)|process\.|Buffer|__dirname|__filename" src/
```

**Expected result:** Zero matches. If matches found, either:
- Refactor to use Web-standard APIs (e.g., `fetch` instead of `http`)
- Use conditional imports with Deno polyfills
- Accept that a separate Deno port is required

### 2. Add JSR Configuration

Create `jsr.json` (or `deno.json`) in package root:

```json
{
  "name": "@scope/package-name",
  "version": "0.0.0-placeholder",
  "exports": "./src/index.ts",
  "publish": {
    "include": [
      "src/**/*.ts",
      "README.md",
      "LICENSE"
    ],
    "exclude": [
      "**/*.test.ts",
      "**/*.spec.ts",
      "tests/**"
    ]
  }
}
```

**Key points:**
- `exports` points to **TypeScript source**, not compiled JS
- `version` is a placeholder; stamp before publish (see §4)
- `publish.include` is an explicit whitelist (JSR best practice)
- `publish.exclude` blocks test files

### 3. Add ESM Build Config (if needed)

If your npm package uses CommonJS, JSR requires ESM. Create `tsconfig.jsr.json`:

```json
{
  "extends": "./tsconfig.json",
  "compilerOptions": {
    "module": "ES2022",
    "target": "ES2022",
    "outDir": "dist-jsr"
  }
}
```

**Note:** You may not need to actually *build* for JSR (it compiles TS natively), but this config is useful for validation.

### 4. Version Stamping Strategy

**Recommended:** Sync npm and JSR versions using your existing version tool (e.g., nerdbank-gitversioning, semantic-release).

**CI implementation (GitHub Actions):**

```yaml
- name: Stamp version (JSR)
  run: |
    VERSION=$(node -p "require('./package.json').version")
    jq --arg v "$VERSION" '.version = $v' jsr.json > jsr.json.tmp
    mv jsr.json.tmp jsr.json

- name: Publish to JSR
  run: deno publish --allow-dirty
  env:
    DENO_DEPLOY_TOKEN: ${{ secrets.DENO_DEPLOY_TOKEN }}

- name: Reset version (JSR)
  if: always()
  run: git checkout -- jsr.json
```

**Why `--allow-dirty`?** Version stamping modifies `jsr.json` after git tag; this flag allows publish despite uncommitted changes.

### 5. Deno Samples (Optional but Recommended)

Create `samples/deno/` (or `examples/deno/`) with Deno-flavored entry points:

**Structure:**
```
samples/
├── example.ts          # Shared logic (Node runtime)
├── deno/
│   └── example.ts      # Deno entry point (JSR import)
```

**Deno entry point template:**

```typescript
// Import from JSR (published package)
import { MyBuilder } from 'jsr:@rido-min/my-package';

// Or, for local testing before publish:
// import { MyBuilder } from '../../src/index.ts';

export function createExample() {
  return MyBuilder.create().build();
}

if (import.meta.main) {
  console.log(JSON.stringify(createExample(), null, 2));
}
```

**Run with:** `deno run samples/deno/example.ts`

### 6. Deno Test Suite (Optional)

Add a Deno test pass to validate JSR package in Deno runtime:

**Option A:** Symlink Node tests, rewrite imports
**Option B:** Create minimal `tests-deno/` with 20+ core tests

**CI step:**

```yaml
- name: Setup Deno
  uses: denoland/setup-deno@v1
  with:
    deno-version: v1.x

- name: Test (Deno)
  run: deno test tests-deno/
```

### 7. CI Integration

Add to `.github/workflows/ci.yml` (or equivalent):

```yaml
jobs:
  publish-jsr:
    runs-on: ubuntu-latest
    if: startsWith(github.ref, 'refs/tags/v')  # Only on version tags
    steps:
      - uses: actions/checkout@v5
      - uses: denoland/setup-deno@v1

      - name: Stamp version
        run: |
          VERSION=${GITHUB_REF#refs/tags/v}
          jq --arg v "$VERSION" '.version = $v' jsr.json > jsr.json.tmp
          mv jsr.json.tmp jsr.json

      - name: Publish to JSR
        run: deno publish --allow-dirty
        env:
          DENO_DEPLOY_TOKEN: ${{ secrets.DENO_DEPLOY_TOKEN }}
```

### 8. Secrets Setup

1. Go to https://jsr.io and create account (or login)
2. Create package scope (e.g., `@yourorg`)
3. Generate publish token: **Settings → Tokens → New Token** (scope: `publish`)
4. Add to GitHub: **Repo Settings → Secrets → Actions → New repository secret**
   - Name: `DENO_DEPLOY_TOKEN`
   - Value: `<token from JSR>`

### 9. Documentation Updates

**README.md:**

```markdown
## Installation

### Node.js / npm
\`\`\`bash
npm install my-package
\`\`\`

### Deno / JSR
\`\`\`bash
deno add jsr:@scope/my-package
\`\`\`

## Usage

### Node.js
\`\`\`typescript
import { MyBuilder } from 'my-package';
\`\`\`

### Deno
\`\`\`typescript
import { MyBuilder } from 'jsr:@scope/my-package';
\`\`\`
```

**AGENTS.md or CONTRIBUTING.md:**

```markdown
### Deno Support (JSR Publication)

This library is published to **both npm and JSR** from a single codebase.

**For Deno consumers:**
- Import from `jsr:@scope/my-package`
- Samples: `deno run samples/deno/example.ts`

**For contributors:**
- The `src/` directory is shared — no code duplication
- JSR config: `jsr.json`
- Publishing: CI auto-publishes on version tags
- Testing: `deno test tests-deno/` (optional)
```

### 10. Manual Publish Test (First Time)

Before relying on CI, test manually:

```bash
# Stamp version
jq '.version = "0.1.0-test"' jsr.json > jsr.json.tmp
mv jsr.json.tmp jsr.json

# Dry run
deno publish --dry-run

# Publish (real)
deno publish

# Verify
open https://jsr.io/@scope/my-package

# Test import
deno add jsr:@scope/my-package
```

---

## Common Pitfalls

1. **Forgetting `--allow-dirty`** — Version stamping modifies files; `deno publish` will fail without this flag
2. **Using Node.js APIs in "pure" TypeScript** — Always grep for `process`, `Buffer`, `fs`, `node:*` before claiming Deno compatibility
3. **Divergent versions** — If npm is 1.2.3 but JSR is 1.2.0, consumers get confused. Always sync.
4. **Missing LICENSE in publish** — JSR infers license from `LICENSE` file; must be in `publish.include` or root
5. **Exposing tests to JSR** — Tests in published package bloat size; always exclude via `publish.exclude`
6. **Hardcoding npm import in samples** — Deno samples must use `jsr:` imports, not `npm:` or bare specifiers

---

## Success Metrics

- ✅ Package live on https://jsr.io/@scope/package-name
- ✅ Deno users can `deno add jsr:@scope/package-name` and import successfully
- ✅ npm and JSR versions are identical (no drift)
- ✅ Zero code duplication between npm and JSR
- ✅ CI publishes to both registries on tag push
- ✅ Documentation covers both npm and JSR installation

---

## When This Pattern Fails

If you encounter:
- **Node.js API dependencies** — Refactor or accept separate Deno port
- **Different API surfaces for Node vs Deno** — Maintain two packages (not ideal but sometimes necessary)
- **Build-time code generation** — JSR expects static TypeScript; if your build step generates code, you may need to publish compiled artifacts

---

## References

- JSR Publishing Guide: https://jsr.io/docs/publishing-packages
- JSR vs npm comparison: https://jsr.io/docs/why
- Deno and JSR: https://docs.deno.com/runtime/fundamentals/modules/#jsr-packages
- Example libraries using dual publication: Zod, Effect, tRPC, Hono

---

## Maintained By

Keaton (Lead / Architect) — FluentCards Project  
Last Updated: 2025-01-23
