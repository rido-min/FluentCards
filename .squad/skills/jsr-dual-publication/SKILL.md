# Skill: JSR Dual-Publication for npm+Deno

**Category:** Build & Packaging
**Applies to:** TypeScript libraries published to both npm and JSR (Deno registry)
**Status:** SUPERSEDED — see revision below
**Confidence:** REVISION REQUIRED

**⚠️ NOTICE (2026-05-04):** The pattern described below (build-jsr.mjs rewrite script) was based on incomplete understanding of Deno's sloppy-imports configuration. Post-implementation testing revealed that a workspace-root `deno.json` with `"unstable": ["sloppy-imports"]` allows JSR to consume TypeScript source with `.js` extensions directly, eliminating the need for the rewrite. See "REVISED PATTERN" section at the bottom.

---

## Problem

You have a pure TypeScript library on npm (CommonJS or NodeNext ESM) and want to publish it to JSR for Deno consumers **without breaking the existing npm build**.

**Two traps to avoid:**

1. Adding `"type": "module"` to `package.json` or switching `tsconfig.json` to emit ESM will break existing CommonJS consumers.
2. Pointing JSR's `exports` directly at `src/*.ts` if the source uses `.js` import extensions (the standard TS ESM pattern). JSR rejects "sloppy imports" — it requires explicit `.ts` extensions on relative imports. `--no-check` does NOT bypass this; JSR's module resolver itself fails.

---

## Solution

JSR consumes TypeScript source directly, but the source must use `.ts` extensions. Since the npm build needs `.js` extensions (so compiled output works), the npm and JSR sources have to differ on this one detail.

The fix: a small build script that copies `src/` to a `jsr-src/` directory, rewriting `from './foo.js'` to `from './foo.ts'` in every relative import. JSR's `exports` then points at `./jsr-src/index.ts`.

### Architecture

```
your-package/
  src/                       # Authoritative source — .js extensions on relative imports
    index.ts
    models.ts
    builders/
  dist/                      # CommonJS compiled output (npm) — gitignored
  jsr-src/                   # Generated JSR-shaped copy (.ts extensions) — gitignored
  scripts/
    build-jsr.mjs            # The rewrite script (Node, no deps)
  tests-deno/                # Deno tests; import from ../src/ with --sloppy-imports
  package.json               # NO "type": "module"; module stays CommonJS
  tsconfig.json              # module: "CommonJS" (or whatever npm needs)
  jsr.json                   # exports: "./jsr-src/index.ts"
  LICENSE                    # Copy into the package dir if not already there (JSR includes it)
```

### `scripts/build-jsr.mjs` (the essential piece)

```js
import { mkdir, readdir, readFile, rm, writeFile } from 'node:fs/promises';
import { dirname, join, relative } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const SRC_DIR = join(__dirname, '..', 'src');
const OUT_DIR = join(__dirname, '..', 'jsr-src');
const IMPORT_RE = /(from\s+['"])(\.\.?\/[^'"]+)\.js(['"])/g;

async function* walk(dir) {
  for (const e of await readdir(dir, { withFileTypes: true })) {
    const p = join(dir, e.name);
    if (e.isDirectory()) yield* walk(p); else if (e.isFile()) yield p;
  }
}

await rm(OUT_DIR, { recursive: true, force: true });
await mkdir(OUT_DIR, { recursive: true });
for await (const src of walk(SRC_DIR)) {
  const out = join(OUT_DIR, relative(SRC_DIR, src));
  await mkdir(dirname(out), { recursive: true });
  if (src.endsWith('.ts')) {
    const text = await readFile(src, 'utf8');
    await writeFile(out, text.replace(IMPORT_RE, (_, p, path, s) => `${p}${path}.ts${s}`));
  } else {
    await writeFile(out, await readFile(src));
  }
}
```

### `jsr.json`

```json
{
  "name": "@your-scope/your-package",
  "version": "0.0.0-placeholder",
  "exports": "./jsr-src/index.ts",
  "publish": {
    "include": ["jsr-src/**/*.ts", "README.md", "LICENSE"],
    "exclude": ["**/*.test.ts", "**/*.spec.ts", "!jsr-src"]
  }
}
```

The `"!jsr-src"` entry in `publish.exclude` is a NEGATIVE glob that overrides any matching `.gitignore` rule. Without it, the gitignored `jsr-src/` tree is invisible to `deno publish`.

### CI ordering (critical)

Run `node scripts/build-jsr.mjs` BEFORE `deno check`, `deno publish --dry-run`, or the actual `deno publish`. Run it again after stamping the version on tagged releases.

### `.gitignore`

Add `jsr-src/`. The directory is generated; never commit it.

---

## Other gotchas you will hit

- **JSR slow-types check.** Object-literal `export const FooApi = { ... }` triggers `missing-explicit-type`. Define an `interface FooApi { ... }` and annotate: `export const FooApi: FooApi = { ... }`. Or pass `--allow-slow-types` (degrades the JSR experience for consumers — prefer the fix).
- **Local Deno samples.** Until the package is published, samples that import `jsr:@scope/package` will fail. Either keep the local relative import as the active line until first publish, or ship a `deno.json` import map for samples that maps the JSR specifier to the local path.
- **`--sloppy-imports` for tests.** The Deno test suite can keep importing from `../src/index.ts` (no need to use `jsr-src/`) as long as you pass `--sloppy-imports` to `deno test`. JSR rules don't apply to tests.
- **Workspace deno.json.** `"unstable": ["sloppy-imports"]` only takes effect at the workspace root, not in a sub-package. Don't try to scope it to one package.
- **Gitignored `jsr-src/` is excluded from publish.** `deno publish` respects `.gitignore` by default, so any `jsr-src/` entry in `.gitignore` blocks the entire generated tree from being published. Add `"!jsr-src"` (negative glob) to `publish.exclude` in `jsr.json` to override the gitignore. `publish.include` alone does NOT override gitignore.
- **JSR scope ownership.** Confirm the JSR scope name with the project owner — it does not have to match npm.

---

## Success Criteria

- npm consumers can install and use the package as before — zero breaking changes
- `deno publish --dry-run` succeeds locally and in CI on every PR
- Deno consumers can `deno add jsr:@scope/package` after first published release
- CI publishes to both npm and JSR on tagged releases
- A single `src/` directory is the source of truth; `jsr-src/` is generated

---

## References

- [JSR Publishing Guide](https://jsr.io/docs/publishing-packages)
- [JSR slow types](https://jsr.io/docs/about-slow-types)
- [Deno sloppy imports](https://docs.deno.com/runtime/reference/cli/unstable_flags/#--unstable-sloppy-imports)
- FluentCards Issue #80 — real-world implementation

---

## REVISED PATTERN (2026-05-04) — RECOMMENDED

**The above pattern (build-jsr.mjs) is no longer necessary.** Empirical testing revealed that JSR accepts `.js` extensions in TypeScript source when configured correctly.

### Correct Solution: Workspace-Root deno.json

**File structure:**
```
your-workspace/
  deno.json                          # ← Workspace root config (CRITICAL)
  packages/
    your-package/
      src/
        index.ts                      # Uses .js extensions (for npm CJS build)
        models.ts
      dist/                           # npm build output (gitignored)
      package.json                    # npm config
      tsconfig.json                   # npm TypeScript config (module: CommonJS)
      jsr.json                        # JSR config
```

**Workspace deno.json:**
```json
{
  "unstable": ["sloppy-imports"]
}
```

**Package jsr.json:**
```json
{
  "name": "@your-scope/your-package",
  "version": "0.0.0-placeholder",
  "exports": "./src/index.ts",
  "publish": {
    "include": ["src/**/*.ts", "README.md", "LICENSE"],
    "exclude": ["**/*.test.ts", "**/*.spec.ts"]
  }
}
```

**That's it.** No build scripts, no jsr-src/ directory, no negative globs, no --sloppy-imports per command.

### Empirical Verification (Deno 2.7.14, 2026-05-04)

**Test 1: JSR publish accepts source with .js extensions**
```bash
cd packages/your-package
deno publish --dry-run --allow-dirty
# ✅ Success: "Simulating publish of @your-scope/your-package..."
```

**Test 2: Deno tests run without --sloppy-imports flag**
```bash
deno test tests-deno/
# ✅ ok | 39 passed | 0 failed (355ms)
```

**Test 3: Local samples can import from src/ directly**
```typescript
// samples/deno/example.ts
import { AdaptiveCardBuilder } from '../../packages/your-package/src/index.ts';
```
```bash
deno run samples/deno/example.ts
# ✅ Executes without errors or flags
```

### Why This Works

JSR's documentation states: "When a package.json is present in your package, modules MAY use 'sloppy imports'." The critical constraint (discovered through testing): the `"unstable": ["sloppy-imports"]` configuration **MUST be at the workspace root**, not in a package-level deno.json. When configured correctly, Deno's module resolver accepts `.js` extensions resolving to `.ts` files for both `deno publish` and `deno test`.

### Migration from build-jsr Pattern

1. Add `deno.json` at workspace root with `{"unstable": ["sloppy-imports"]}`
2. Change `jsr.json` exports from `./jsr-src/index.ts` to `./src/index.ts`
3. Delete `scripts/build-jsr.mjs`
4. Remove `jsr-src/` from `.gitignore`
5. Remove "Build JSR sources" steps from CI
6. Remove `--sloppy-imports` flag from `deno test` commands
7. Remove `"!jsr-src"` negative glob from `jsr.json` publish.exclude

### Updated Success Criteria

- ✅ npm consumers can install and use the package as before — zero breaking changes
- ✅ `deno publish --dry-run` succeeds without build scripts
- ✅ Deno consumers can `deno add jsr:@scope/package` after first published release
- ✅ CI publishes to both npm and JSR on tagged releases
- ✅ **Single `src/` directory is the source of truth — no generated directories**
- ✅ **Local Deno samples work without import swapping**
- ✅ **Deno tests run without per-command flags**

### References for Revised Pattern

- [Deno sloppy imports documentation](https://docs.deno.com/runtime/reference/cli/unstable_flags/#--unstable-sloppy-imports) — note workspace-root requirement
- [JSR publishing with package.json present](https://jsr.io/docs/publishing-packages) — mentions sloppy imports are allowed when package.json exists
- FluentCards Issue #80 architecture rethink: `.squad/decisions/inbox/keaton-issue-80-rethink.md`
