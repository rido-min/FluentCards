# Skill: JSR Dual-Publication for npm+Deno

**Category:** Build & Packaging  
**Applies to:** TypeScript libraries published to both npm and JSR (Deno registry)  
**Status:** Proven (FluentCards node/ port, Issue #80)

---

## Problem

You have a pure TypeScript library on npm (CommonJS) and want to publish it to JSR for Deno consumers **without breaking the existing npm build**.

**The trap:** Adding "type": "module" to package.json or changing 	sconfig.json to emit ESM will break CommonJS consumers.

---

## Solution

**Key insight:** JSR can consume TypeScript source (.ts) directly via the xports field in jsr.json. This avoids needing to change the npm build configuration.

### Architecture

``
your-package/
├── src/
│   ├── index.ts          # ESM source (import/export)
│   ├── models.ts
│   └── builders/
├── dist/                  # CommonJS compiled output (npm)
├── package.json           # NO "type": "module", module: CommonJS
├── tsconfig.json          # module: "CommonJS" (npm build)
├── jsr.json               # JSR config, exports: "./src/index.ts"
└── LICENSE                # Copy from root if needed
``

**Dual-publication flow:**
- **npm:** Consumes dist/ (CommonJS, compiled from src/ via tsc)
- **JSR:** Consumes src/ (ESM, native TypeScript)

---

## Key Insight

The "consume source directly" pattern solves the CommonJS/ESM conflict. JSR reads .ts files natively, getting ESM semantics from TypeScript syntax, while npm continues to use compiled CommonJS output.

---

## Success Criteria

- ✅ npm consumers can install and use package as before (zero breaking changes)
- ✅ Deno consumers can deno add jsr:@scope/package and import directly
- ✅ CI publishes to both npm and JSR on tagged releases
- ✅ Single source of truth (src/) for both runtimes
- ✅ Samples work in both Node and Deno

---

## References

- [JSR Publishing Guide](https://jsr.io/docs/publishing-packages)
- [Deno Module System](https://docs.deno.com/runtime/fundamentals/modules/)
- [FluentCards Issue #80](https://github.com/rido-min/FluentCards/issues/80) — Real-world implementation
