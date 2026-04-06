import type {
  AdaptiveAction,
  OpenUrlAction,
  SubmitAction,
  ShowCardAction,
  ToggleVisibilityAction,
  ExecuteAction,
  TargetElement,
  AdaptiveCard,
} from '../models.js';
import { ActionStyle, AssociatedInputs } from '../enums.js';

/**
 * Fluent builder for {@link AdaptiveAction} instances.
 *
 * Call one of the action-type methods first (`openUrl`, `submit`, etc.)
 * then chain modifiers, then call `build()`.
 */
export class ActionBuilder {
  private action: AdaptiveAction | undefined;

  /** Sets the action type to OpenUrl. @param url The URL to open. @param title Optional button title. @returns The builder instance for method chaining. */
  openUrl(url: string, title?: string): this {
    this.action = { type: 'Action.OpenUrl', url, title } as OpenUrlAction;
    return this;
  }

  /** Sets the action type to Submit. @param title Optional button title. @returns The builder instance for method chaining. */
  submit(title?: string): this {
    this.action = { type: 'Action.Submit', title } as SubmitAction;
    return this;
  }

  /** Sets the action type to ShowCard. @param title Optional button title. @returns The builder instance for method chaining. */
  showCard(title?: string): this {
    this.action = { type: 'Action.ShowCard', title } as ShowCardAction;
    return this;
  }

  /** Sets the action type to ToggleVisibility. @param title Optional button title. @returns The builder instance for method chaining. */
  toggleVisibility(title?: string): this {
    this.action = { type: 'Action.ToggleVisibility', title } as ToggleVisibilityAction;
    return this;
  }

  /** Sets the action type to Execute (Universal Action Model). @param title Optional button title. @returns The builder instance for method chaining. */
  execute(title?: string): this {
    this.action = { type: 'Action.Execute', title } as ExecuteAction;
    return this;
  }

  /** Sets the unique identifier for the action. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this {
    if (this.action) this.action.id = id;
    return this;
  }

  /** Sets the button title. @param title The button label text. @returns The builder instance for method chaining. */
  withTitle(title: string): this {
    if (this.action) this.action.title = title;
    return this;
  }

  /** Sets the icon URL displayed on the button. @param iconUrl The icon URL. @returns The builder instance for method chaining. */
  withIconUrl(iconUrl: string): this {
    if (this.action) this.action.iconUrl = iconUrl;
    return this;
  }

  /** Sets the visual style of the action button. @param style The action style. @returns The builder instance for method chaining. */
  withStyle(style: ActionStyle): this {
    if (this.action) this.action.style = style;
    return this;
  }

  /** Sets whether the action is enabled. @param isEnabled True to enable the action. @returns The builder instance for method chaining. */
  withIsEnabled(isEnabled: boolean): this {
    if (this.action) this.action.isEnabled = isEnabled;
    return this;
  }

  /** Sets the tooltip shown on hover. @param tooltip The tooltip text. @returns The builder instance for method chaining. */
  withTooltip(tooltip: string): this {
    if (this.action) this.action.tooltip = tooltip;
    return this;
  }

  // ─── SubmitAction / ExecuteAction specifics ───────────────────────────────

  /** Sets the data payload (for Submit or Execute actions). @param data The data to submit. @returns The builder instance for method chaining. */
  withData(data: unknown): this {
    if (this.action?.type === 'Action.Submit' || this.action?.type === 'Action.Execute') {
      (this.action as SubmitAction | ExecuteAction).data = data;
    }
    return this;
  }

  /** Sets which inputs are included when the action is submitted (for Submit or Execute actions). @param associatedInputs The associated inputs option. @returns The builder instance for method chaining. */
  withAssociatedInputs(associatedInputs: AssociatedInputs): this {
    if (this.action?.type === 'Action.Submit' || this.action?.type === 'Action.Execute') {
      (this.action as SubmitAction | ExecuteAction).associatedInputs = associatedInputs;
    }
    return this;
  }

  // ─── ExecuteAction specific ───────────────────────────────────────────────

  /** Sets the verb for an Execute action. @param verb The command verb. @returns The builder instance for method chaining. */
  withVerb(verb: string): this {
    if (this.action?.type === 'Action.Execute') {
      (this.action as ExecuteAction).verb = verb;
    }
    return this;
  }

  // ─── ShowCardAction specific ──────────────────────────────────────────────

  /** Sets the card to reveal for a ShowCard action. @param card The embedded AdaptiveCard. @returns The builder instance for method chaining. */
  withCard(card: AdaptiveCard): this {
    if (this.action?.type === 'Action.ShowCard') {
      (this.action as ShowCardAction).card = card;
    }
    return this;
  }

  // ─── ToggleVisibilityAction specific ─────────────────────────────────────

  /** Adds a target element to toggle for a ToggleVisibility action. @param elementId The ID of the element to toggle. @param isVisible Optional forced visibility state. @returns The builder instance for method chaining. */
  addTargetElement(elementId: string, isVisible?: boolean): this {
    if (this.action?.type === 'Action.ToggleVisibility') {
      const a = this.action as ToggleVisibilityAction;
      a.targetElements ??= [];
      if (isVisible === undefined) {
        a.targetElements.push(elementId);
      } else {
        a.targetElements.push({ elementId, isVisible });
      }
    }
    return this;
  }

  /** Builds and returns the configured action. @returns The configured AdaptiveAction instance. @throws Error if no action type has been set. */
  build(): AdaptiveAction {
    if (!this.action) {
      throw new Error(
        'No action type specified. Call openUrl(), submit(), showCard(), toggleVisibility(), or execute() first.',
      );
    }
    return this.action;
  }
}
