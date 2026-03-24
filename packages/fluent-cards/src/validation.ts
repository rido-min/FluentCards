import type { AdaptiveCard, AdaptiveElement, AdaptiveAction } from './models.js';
import { ValidationSeverity } from './enums.js';

export { ValidationSeverity };

/** A single validation finding. */
export interface ValidationIssue {
  readonly severity: ValidationSeverity;
  /** JSON path to the problematic property, e.g. `"body[0].url"`. */
  readonly path: string;
  /** Short machine-readable code, e.g. `"MISSING_IMAGE_URL"`. */
  readonly code: string;
  /** Human-readable description. */
  readonly message: string;
}

/** Thrown by {@link validateAndThrow} when errors are found. */
export class AdaptiveCardValidationError extends Error {
  constructor(public readonly errors: ValidationIssue[]) {
    super(formatMessage(errors));
    this.name = 'AdaptiveCardValidationError';
  }
}

function formatMessage(errors: ValidationIssue[]): string {
  if (errors.length === 1) {
    return `Adaptive Card validation failed: ${errors[0].message}`;
  }
  const lines = errors.map((e) => `  - [${e.path}] ${e.message}`).join('\n');
  return `Adaptive Card validation failed with ${errors.length} errors:\n${lines}`;
}

// ─── Validator ───────────────────────────────────────────────────────────────

/** Validate a card and return all findings (errors + warnings). */
export function validate(card: AdaptiveCard): ValidationIssue[] {
  const issues: ValidationIssue[] = [];
  validateCard(card, issues);
  return issues;
}

/**
 * Validate a card and throw {@link AdaptiveCardValidationError} if any
 * error-severity issues are found.  Warnings are silently ignored.
 */
export function validateAndThrow(card: AdaptiveCard): void {
  const errors = validate(card).filter((i) => i.severity === ValidationSeverity.Error);
  if (errors.length > 0) throw new AdaptiveCardValidationError(errors);
}

// ─── Internal helpers ─────────────────────────────────────────────────────────

const KNOWN_VERSIONS = new Set(['1.0', '1.1', '1.2', '1.3', '1.4', '1.5', '1.6']);

function issue(
  issues: ValidationIssue[],
  severity: ValidationSeverity,
  path: string,
  code: string,
  message: string,
): void {
  issues.push({ severity, path, code, message });
}

function validateCard(card: AdaptiveCard, issues: ValidationIssue[]): void {
  if (!card['$schema']) {
    issue(
      issues,
      ValidationSeverity.Warning,
      '$schema',
      'MISSING_SCHEMA',
      "The '$schema' property is missing. While optional, including it enables better tooling support.",
    );
  }

  if (!card.version) {
    issue(
      issues,
      ValidationSeverity.Error,
      'version',
      'MISSING_VERSION',
      "The 'version' property is required. Use a value like '1.5' to specify the schema version.",
    );
  } else if (!KNOWN_VERSIONS.has(card.version)) {
    issue(
      issues,
      ValidationSeverity.Warning,
      'version',
      'UNKNOWN_VERSION',
      `The version '${card.version}' is not a known Adaptive Cards version. Known versions: ${[...KNOWN_VERSIONS].join(', ')}.`,
    );
  }

  if (!card.body?.length && !card.actions?.length) {
    issue(
      issues,
      ValidationSeverity.Warning,
      '',
      'EMPTY_CARD',
      'The card has no body elements and no actions. It will render as empty.',
    );
  }

  if (card.body) validateElements(card.body, issues, 'body');

  if (card.actions) {
    validateActions(card.actions, issues, 'actions');
    if (card.actions.length > 5) {
      issue(
        issues,
        ValidationSeverity.Warning,
        'actions',
        'TOO_MANY_ACTIONS',
        `The card has ${card.actions.length} actions. Some hosts limit the number of visible actions to 5.`,
      );
    }
  }
}

function validateElements(elements: AdaptiveElement[], issues: ValidationIssue[], path: string): void {
  elements.forEach((element, i) => validateElement(element, issues, `${path}[${i}]`));
}

function validateElement(element: AdaptiveElement, issues: ValidationIssue[], path: string): void {
  switch (element.type) {
    case 'TextBlock':
      if (!element.text) {
        issue(
          issues,
          ValidationSeverity.Warning,
          `${path}.text`,
          'EMPTY_TEXT',
          'TextBlock has empty or null text. It will render as invisible.',
        );
      }
      break;

    case 'Image':
      if (!element.url) {
        issue(issues, ValidationSeverity.Error, `${path}.url`, 'MISSING_IMAGE_URL', "Image element is missing the required 'url' property.");
      } else if (!isAbsoluteUrl(element.url)) {
        issue(issues, ValidationSeverity.Warning, `${path}.url`, 'INVALID_IMAGE_URL', `Image URL '${element.url}' is not a valid absolute URL.`);
      }
      break;

    case 'Input.Text':
    case 'Input.Number':
    case 'Input.Date':
    case 'Input.Time':
    case 'Input.Toggle':
    case 'Input.ChoiceSet':
      if (!element.id) {
        issue(
          issues,
          ValidationSeverity.Error,
          `${path}.id`,
          'MISSING_INPUT_ID',
          "Input element is missing the required 'id' property. Inputs cannot be submitted without an id.",
        );
      }
      break;

    case 'Container':
      if (element.items) validateElements(element.items, issues, `${path}.items`);
      break;

    case 'ColumnSet':
      element.columns?.forEach((col, i) => {
        if (col.items) validateElements(col.items, issues, `${path}.columns[${i}].items`);
      });
      break;
  }
}

function validateActions(actions: AdaptiveAction[], issues: ValidationIssue[], path: string): void {
  actions.forEach((action, i) => validateAction(action, issues, `${path}[${i}]`));
}

function validateAction(action: AdaptiveAction, issues: ValidationIssue[], path: string): void {
  switch (action.type) {
    case 'Action.OpenUrl':
      if (!action.url) {
        issue(issues, ValidationSeverity.Error, `${path}.url`, 'MISSING_ACTION_URL', "Action.OpenUrl is missing the required 'url' property.");
      } else if (!isAbsoluteUrl(action.url)) {
        issue(issues, ValidationSeverity.Warning, `${path}.url`, 'INVALID_ACTION_URL', `Action.OpenUrl URL '${action.url}' is not a valid absolute URL.`);
      }
      break;

    case 'Action.ShowCard':
      if (!action.card) {
        issue(issues, ValidationSeverity.Error, `${path}.card`, 'MISSING_SHOWCARD', "Action.ShowCard is missing the required 'card' property.");
      } else {
        validateCard(action.card, issues);
      }
      break;
  }
}

function isAbsoluteUrl(url: string): boolean {
  try {
    new URL(url);
    return true;
  } catch {
    return false;
  }
}
