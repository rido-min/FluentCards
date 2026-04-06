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
  const ids = new Set<string>();
  validateCard(card, issues, ids);
  if (card.version && KNOWN_VERSIONS.has(card.version)) {
    validateVersionMismatch(card, card.version, issues);
  }
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

function trackId(id: string | undefined, path: string, issues: ValidationIssue[], ids: Set<string>): void {
  if (!id) return;
  if (ids.has(id)) {
    issue(issues, ValidationSeverity.Warning, path, 'DUPLICATE_ID',
      `Duplicate id '${id}' found. Element IDs should be unique within a card.`);
  } else {
    ids.add(id);
  }
}

function validateCard(card: AdaptiveCard, issues: ValidationIssue[], ids: Set<string>): void {
  if (!card['$schema']) {
    issue(issues, ValidationSeverity.Warning, '$schema', 'MISSING_SCHEMA',
      "The '$schema' property is missing. While optional, including it enables better tooling support.");
  }

  if (!card.version) {
    issue(issues, ValidationSeverity.Error, 'version', 'MISSING_VERSION',
      "The 'version' property is required. Use a value like '1.5' to specify the schema version.");
  } else if (!KNOWN_VERSIONS.has(card.version)) {
    issue(issues, ValidationSeverity.Warning, 'version', 'UNKNOWN_VERSION',
      `The version '${card.version}' is not a known Adaptive Cards version. Known versions: ${[...KNOWN_VERSIONS].join(', ')}.`);
  }

  if (!card.body?.length && !card.actions?.length) {
    issue(issues, ValidationSeverity.Warning, '', 'EMPTY_CARD',
      'The card has no body elements and no actions. It will render as empty.');
  }

  if (card.body) validateElements(card.body, issues, 'body', ids);

  if (card.actions) {
    validateActions(card.actions, issues, 'actions', ids);
    if (card.actions.length > 5) {
      issue(issues, ValidationSeverity.Warning, 'actions', 'TOO_MANY_ACTIONS',
        `The card has ${card.actions.length} actions. Some hosts limit the number of visible actions to 5.`);
    }
  }

  validateSelectAction(card.selectAction, issues, 'selectAction');
}

function validateElements(elements: AdaptiveElement[], issues: ValidationIssue[], path: string, ids: Set<string>): void {
  elements.forEach((element, i) => {
    const elementPath = `${path}[${i}]`;
    trackId(element.id, elementPath, issues, ids);
    validateElement(element, issues, elementPath, ids);
  });
}

function validateElement(element: AdaptiveElement, issues: ValidationIssue[], path: string, ids: Set<string>): void {
  switch (element.type) {
    case 'TextBlock':
      if (!element.text) {
        issue(issues, ValidationSeverity.Error, `${path}.text`, 'MISSING_TEXT',
          "TextBlock is missing the required 'text' property.");
      }
      break;

    case 'Image':
      if (!element.url) {
        issue(issues, ValidationSeverity.Error, `${path}.url`, 'MISSING_IMAGE_URL',
          "Image element is missing the required 'url' property.");
      } else if (!isAbsoluteUrl(element.url)) {
        issue(issues, ValidationSeverity.Warning, `${path}.url`, 'INVALID_IMAGE_URL',
          `Image URL '${element.url}' is not a valid absolute URL.`);
      }
      validateSelectAction(element.selectAction, issues, `${path}.selectAction`);
      break;

    case 'ImageSet':
      if (!element.images?.length) {
        issue(issues, ValidationSeverity.Error, `${path}.images`, 'MISSING_IMAGES',
          "ImageSet is missing the required 'images' property.");
      } else {
        element.images.forEach((img, i) => {
          const imgPath = `${path}.images[${i}]`;
          if (!img.url) {
            issue(issues, ValidationSeverity.Error, `${imgPath}.url`, 'MISSING_IMAGE_URL',
              "Image element is missing the required 'url' property.");
          }
        });
      }
      break;

    case 'FactSet':
      if (!element.facts?.length) {
        issue(issues, ValidationSeverity.Error, `${path}.facts`, 'MISSING_FACTS',
          "FactSet is missing the required 'facts' property.");
      }
      break;

    case 'ActionSet':
      if (!element.actions?.length) {
        issue(issues, ValidationSeverity.Error, `${path}.actions`, 'MISSING_ACTIONSET_ACTIONS',
          "ActionSet is missing the required 'actions' property.");
      } else {
        validateActions(element.actions, issues, `${path}.actions`, ids);
      }
      break;

    case 'RichTextBlock':
      if (!element.inlines?.length) {
        issue(issues, ValidationSeverity.Error, `${path}.inlines`, 'MISSING_INLINES',
          "RichTextBlock is missing the required 'inlines' property.");
      }
      break;

    case 'Media':
      if (!element.sources?.length) {
        issue(issues, ValidationSeverity.Error, `${path}.sources`, 'MISSING_MEDIA_SOURCES',
          "Media is missing the required 'sources' property.");
      }
      break;

    case 'Input.Text':
    case 'Input.Number':
    case 'Input.Date':
    case 'Input.Time':
    case 'Input.Toggle':
    case 'Input.ChoiceSet':
      if (!element.id) {
        issue(issues, ValidationSeverity.Error, `${path}.id`, 'MISSING_INPUT_ID',
          "Input element is missing the required 'id' property. Inputs cannot be submitted without an id.");
      } else {
        trackId(element.id, path, issues, ids);
      }
      validateInputElement(element, issues, path);
      break;

    case 'Container':
      if (!element.items?.length) {
        issue(issues, ValidationSeverity.Warning, `${path}.items`, 'EMPTY_CONTAINER',
          'Container has no items. It will render as empty.');
      } else {
        validateElements(element.items, issues, `${path}.items`, ids);
      }
      validateSelectAction(element.selectAction, issues, `${path}.selectAction`);
      break;

    case 'ColumnSet':
      element.columns?.forEach((col, i) => {
        const colPath = `${path}.columns[${i}]`;
        trackId(col.id, colPath, issues, ids);
        if (col.items) validateElements(col.items, issues, `${colPath}.items`, ids);
        validateSelectAction(col.selectAction, issues, `${colPath}.selectAction`);
      });
      validateSelectAction(element.selectAction, issues, `${path}.selectAction`);
      break;

    case 'Table':
      element.rows?.forEach((row, r) => {
        row.cells?.forEach((cell, c) => {
          if (cell.items) validateElements(cell.items, issues, `${path}.rows[${r}].cells[${c}].items`, ids);
          validateSelectAction(cell.selectAction, issues, `${path}.rows[${r}].cells[${c}].selectAction`);
        });
      });
      break;
  }
}

function validateInputElement(element: AdaptiveElement, issues: ValidationIssue[], path: string): void {
  if (element.type === 'Input.Number') {
    if (element.min !== undefined && element.max !== undefined && element.min > element.max) {
      issue(issues, ValidationSeverity.Error, path, 'MIN_GREATER_THAN_MAX',
        `Input.Number 'min' (${element.min}) is greater than 'max' (${element.max}).`);
    }
  } else if (element.type === 'Input.Date') {
    if (element.min && element.max && element.min > element.max) {
      issue(issues, ValidationSeverity.Error, path, 'MIN_GREATER_THAN_MAX',
        `Input.Date 'min' (${element.min}) is greater than 'max' (${element.max}).`);
    }
  } else if (element.type === 'Input.Time') {
    if (element.min && element.max && element.min > element.max) {
      issue(issues, ValidationSeverity.Error, path, 'MIN_GREATER_THAN_MAX',
        `Input.Time 'min' (${element.min}) is greater than 'max' (${element.max}).`);
    }
  } else if (element.type === 'Input.Toggle') {
    if (!element.title) {
      issue(issues, ValidationSeverity.Error, `${path}.title`, 'MISSING_TOGGLE_TITLE',
        "Input.Toggle is missing the required 'title' property.");
    }
  }
}

function validateSelectAction(action: AdaptiveAction | undefined, issues: ValidationIssue[], path: string): void {
  if (!action) return;
  if (action.type === 'Action.ShowCard') {
    issue(issues, ValidationSeverity.Error, path, 'INVALID_SELECT_ACTION',
      'Action.ShowCard is not allowed as a selectAction. Use Action.OpenUrl, Action.Submit, Action.Execute, or Action.ToggleVisibility.');
  }
}

function validateActions(actions: AdaptiveAction[], issues: ValidationIssue[], path: string, ids: Set<string>): void {
  actions.forEach((action, i) => {
    const actionPath = `${path}[${i}]`;
    trackId(action.id, actionPath, issues, ids);
    validateAction(action, issues, actionPath, ids);
  });
}

function validateAction(action: AdaptiveAction, issues: ValidationIssue[], path: string, ids: Set<string>): void {
  switch (action.type) {
    case 'Action.OpenUrl':
      if (!action.url) {
        issue(issues, ValidationSeverity.Error, `${path}.url`, 'MISSING_ACTION_URL',
          "Action.OpenUrl is missing the required 'url' property.");
      } else if (!isAbsoluteUrl(action.url)) {
        issue(issues, ValidationSeverity.Warning, `${path}.url`, 'INVALID_ACTION_URL',
          `Action.OpenUrl URL '${action.url}' is not a valid absolute URL.`);
      }
      break;

    case 'Action.ShowCard':
      if (!action.card) {
        issue(issues, ValidationSeverity.Error, `${path}.card`, 'MISSING_SHOWCARD',
          "Action.ShowCard is missing the required 'card' property.");
      } else {
        validateCard(action.card, issues, ids);
      }
      break;

    case 'Action.ToggleVisibility':
      if (!action.targetElements?.length) {
        issue(issues, ValidationSeverity.Error, `${path}.targetElements`, 'MISSING_TARGET_ELEMENTS',
          "Action.ToggleVisibility is missing the required 'targetElements' property.");
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

// ─── Version-aware validation ─────────────────────────────────────────────────

/** Maps element/action types to the minimum Adaptive Cards version that introduced them. */
const ELEMENT_VERSIONS: Record<string, string> = {
  // V1.0
  TextBlock: '1.0', Image: '1.0', Container: '1.0', ColumnSet: '1.0',
  FactSet: '1.0', ImageSet: '1.0', Column: '1.0', Fact: '1.0', Choice: '1.0',
  'Action.OpenUrl': '1.0', 'Action.Submit': '1.0', 'Action.ShowCard': '1.0',
  'Input.Text': '1.0', 'Input.Number': '1.0', 'Input.Date': '1.0',
  'Input.Time': '1.0', 'Input.Toggle': '1.0', 'Input.ChoiceSet': '1.0',
  // V1.1
  Media: '1.1',
  // V1.2
  RichTextBlock: '1.2', ActionSet: '1.2', 'Action.ToggleVisibility': '1.2',
  // V1.4
  'Action.Execute': '1.4',
  // V1.5
  Table: '1.5',
};

/** Maps card-level property names to the minimum version that introduced them. */
const CARD_PROPERTY_VERSIONS: Record<string, string> = {
  selectAction: '1.1',
  minHeight: '1.2', verticalContentAlignment: '1.2', backgroundImage: '1.2',
  refresh: '1.4', authentication: '1.4',
  rtl: '1.5',
  metadata: '1.6',
};

function versionToNum(v: string): number {
  const [, minor] = v.split('.').map(Number);
  return minor ?? 0;
}

function versionMismatch(
  issues: ValidationIssue[],
  path: string,
  featureName: string,
  requiredVersion: string,
  cardVersion: string,
): void {
  issue(issues, ValidationSeverity.Warning, path, 'VERSION_MISMATCH',
    `'${featureName}' requires Adaptive Cards ${requiredVersion} but card version is ${cardVersion}.`);
}

function checkElementVersion(type: string, cardVersion: string, issues: ValidationIssue[], path: string): void {
  const required = ELEMENT_VERSIONS[type];
  if (required && versionToNum(required) > versionToNum(cardVersion)) {
    versionMismatch(issues, path, type, required, cardVersion);
  }
}

function checkCardPropertyVersion(prop: string, cardVersion: string, issues: ValidationIssue[]): void {
  const required = CARD_PROPERTY_VERSIONS[prop];
  if (required && versionToNum(required) > versionToNum(cardVersion)) {
    versionMismatch(issues, prop, prop, required, cardVersion);
  }
}

function validateVersionMismatch(card: AdaptiveCard, cardVersion: string, issues: ValidationIssue[]): void {
  if (card.selectAction) checkCardPropertyVersion('selectAction', cardVersion, issues);
  if (card.minHeight) checkCardPropertyVersion('minHeight', cardVersion, issues);
  if (card.verticalContentAlignment) checkCardPropertyVersion('verticalContentAlignment', cardVersion, issues);
  if (card.backgroundImage) checkCardPropertyVersion('backgroundImage', cardVersion, issues);
  if (card.refresh) checkCardPropertyVersion('refresh', cardVersion, issues);
  if (card.authentication) checkCardPropertyVersion('authentication', cardVersion, issues);
  if (card.rtl !== undefined) checkCardPropertyVersion('rtl', cardVersion, issues);
  if (card.metadata) checkCardPropertyVersion('metadata', cardVersion, issues);

  if (card.body) checkElementVersionsInList(card.body, cardVersion, issues, 'body');
  if (card.actions) checkActionVersionsInList(card.actions, cardVersion, issues, 'actions');
}

function checkElementVersionsInList(elements: AdaptiveElement[], cardVersion: string, issues: ValidationIssue[], path: string): void {
  elements.forEach((el, i) => {
    const p = `${path}[${i}]`;
    checkElementVersion(el.type, cardVersion, issues, p);
    switch (el.type) {
      case 'Container':
        if (el.items) checkElementVersionsInList(el.items, cardVersion, issues, `${p}.items`);
        break;
      case 'ColumnSet':
        el.columns?.forEach((col, ci) => {
          if (col.items) checkElementVersionsInList(col.items, cardVersion, issues, `${p}.columns[${ci}].items`);
        });
        break;
      case 'ActionSet':
        if (el.actions) checkActionVersionsInList(el.actions, cardVersion, issues, `${p}.actions`);
        break;
      case 'Table':
        el.rows?.forEach((row, r) => {
          row.cells?.forEach((cell, c) => {
            if (cell.items) checkElementVersionsInList(cell.items, cardVersion, issues, `${p}.rows[${r}].cells[${c}].items`);
          });
        });
        break;
    }
  });
}

function checkActionVersionsInList(actions: AdaptiveAction[], cardVersion: string, issues: ValidationIssue[], path: string): void {
  actions.forEach((action, i) => {
    const p = `${path}[${i}]`;
    checkElementVersion(action.type, cardVersion, issues, p);
    if (action.type === 'Action.ShowCard' && action.card) {
      if (action.card.body) checkElementVersionsInList(action.card.body, cardVersion, issues, `${p}.card.body`);
      if (action.card.actions) checkActionVersionsInList(action.card.actions, cardVersion, issues, `${p}.card.actions`);
    }
  });
}
