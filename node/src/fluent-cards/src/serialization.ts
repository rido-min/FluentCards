import type { AdaptiveCard } from './models.js';

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
