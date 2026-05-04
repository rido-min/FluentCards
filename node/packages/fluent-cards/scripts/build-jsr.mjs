// Prepares a JSR-publishable copy of src/ by rewriting `.js` extensions in
// relative imports/exports to `.ts`. JSR rejects sloppy imports, so we cannot
// publish src/ directly — npm builds need `.js` extensions for compiled CJS
// output, while JSR consumes TS source and requires explicit `.ts` extensions.
//
// Output: jsr-src/ (gitignored). Run before any `deno check`, `deno test`
// against jsr-src, or `deno publish`. CI invokes this automatically.
//
// Usage: node scripts/build-jsr.mjs

import { mkdir, readdir, readFile, rm, writeFile } from 'node:fs/promises';
import { dirname, join, relative } from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = dirname(fileURLToPath(import.meta.url));
const PACKAGE_ROOT = join(__dirname, '..');
const SRC_DIR = join(PACKAGE_ROOT, 'src');
const OUT_DIR = join(PACKAGE_ROOT, 'jsr-src');

// Match `from '...'` and `from "..."` in import/export statements where the
// specifier is relative (./ or ../) and ends with .js. Capture the path so
// we can rewrite the extension only.
const IMPORT_RE = /(from\s+['"])(\.\.?\/[^'"]+)\.js(['"])/g;

async function* walk(dir) {
  const entries = await readdir(dir, { withFileTypes: true });
  for (const entry of entries) {
    const path = join(dir, entry.name);
    if (entry.isDirectory()) yield* walk(path);
    else if (entry.isFile()) yield path;
  }
}

async function main() {
  await rm(OUT_DIR, { recursive: true, force: true });
  await mkdir(OUT_DIR, { recursive: true });

  let fileCount = 0;
  let rewriteCount = 0;

  for await (const srcPath of walk(SRC_DIR)) {
    const rel = relative(SRC_DIR, srcPath);
    const outPath = join(OUT_DIR, rel);
    await mkdir(dirname(outPath), { recursive: true });

    if (srcPath.endsWith('.ts')) {
      const original = await readFile(srcPath, 'utf8');
      const rewritten = original.replace(IMPORT_RE, (_, prefix, path, suffix) => {
        rewriteCount++;
        return `${prefix}${path}.ts${suffix}`;
      });
      await writeFile(outPath, rewritten, 'utf8');
    } else {
      const buf = await readFile(srcPath);
      await writeFile(outPath, buf);
    }
    fileCount++;
  }

  console.log(`build-jsr: copied ${fileCount} files, rewrote ${rewriteCount} import extensions to .ts`);
  console.log(`build-jsr: output -> ${relative(PACKAGE_ROOT, OUT_DIR)}/`);
}

main().catch((err) => {
  console.error('build-jsr failed:', err);
  process.exit(1);
});
