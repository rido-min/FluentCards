import type { AdaptiveCard } from './models.js';

/**
 * Recursively strip `undefined` values from a plain object tree.
 *
 * Arrays are preserved (with their elements cleaned). Primitive values
 * pass through unchanged. The result is a new object — the input is
 * never mutated.
 */
function stripUndefined(value: unknown): unknown {
  if (value === null || value === undefined) return value;
  if (Array.isArray(value)) return value.map(stripUndefined);
  if (typeof value === 'object') {
    const clean: Record<string, unknown> = {};
    for (const [k, v] of Object.entries(value as Record<string, unknown>)) {
      if (v !== undefined) {
        clean[k] = stripUndefined(v);
      }
    }
    return clean;
  }
  return value;
}

/**
 * Serialize an {@link AdaptiveCard} to a JSON string.
 *
 * `undefined` values are automatically omitted by `JSON.stringify`.
 * Enum values are already the correct camelCase strings, so no custom
 * converter is needed — it just works.
 */
export function toJson(card: AdaptiveCard, indent = 2): string {
  return JSON.stringify(card, undefined, indent);
}

/**
 * Return a clean plain object for an {@link AdaptiveCard}, with all
 * `undefined` properties stripped.
 *
 * Use this instead of {@link toJson} when you need a native object — for
 * example, to embed a card inside a larger API payload without double-
 * serializing.
 */
export function toObject(card: AdaptiveCard): AdaptiveCard {
  return stripUndefined(card) as AdaptiveCard;
}

/**
 * Deserialize a JSON string to an {@link AdaptiveCard}.
 *
 * Because the Adaptive Card schema embeds a `type` discriminant on every
 * element, the parsed object already carries the right shape.  TypeScript's
 * structural typing lets callers narrow with `element.type === 'TextBlock'`
 * etc. without needing class instances.
 *
 * Returns `null` when the input is empty or not a valid JSON object.
 */
export function fromJson(json: string): AdaptiveCard | null {
  try {
    const parsed = JSON.parse(json);
    if (parsed && typeof parsed === 'object' && parsed.type === 'AdaptiveCard') {
      return parsed as AdaptiveCard;
    }
    return null;
  } catch {
    return null;
  }
}
