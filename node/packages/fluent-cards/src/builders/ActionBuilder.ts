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

  openUrl(url: string, title?: string): this {
    this.action = { type: 'Action.OpenUrl', url, title } as OpenUrlAction;
    return this;
  }

  submit(title?: string): this {
    this.action = { type: 'Action.Submit', title } as SubmitAction;
    return this;
  }

  showCard(title?: string): this {
    this.action = { type: 'Action.ShowCard', title } as ShowCardAction;
    return this;
  }

  toggleVisibility(title?: string): this {
    this.action = { type: 'Action.ToggleVisibility', title } as ToggleVisibilityAction;
    return this;
  }

  execute(title?: string): this {
    this.action = { type: 'Action.Execute', title } as ExecuteAction;
    return this;
  }

  withId(id: string): this {
    if (this.action) this.action.id = id;
    return this;
  }

  withTitle(title: string): this {
    if (this.action) this.action.title = title;
    return this;
  }

  withIconUrl(iconUrl: string): this {
    if (this.action) this.action.iconUrl = iconUrl;
    return this;
  }

  withStyle(style: ActionStyle): this {
    if (this.action) this.action.style = style;
    return this;
  }

  withIsEnabled(isEnabled: boolean): this {
    if (this.action) this.action.isEnabled = isEnabled;
    return this;
  }

  withTooltip(tooltip: string): this {
    if (this.action) this.action.tooltip = tooltip;
    return this;
  }

  // ─── SubmitAction / ExecuteAction specifics ───────────────────────────────

  withData(data: unknown): this {
    if (this.action?.type === 'Action.Submit' || this.action?.type === 'Action.Execute') {
      (this.action as SubmitAction | ExecuteAction).data = data;
    }
    return this;
  }

  withAssociatedInputs(associatedInputs: AssociatedInputs): this {
    if (this.action?.type === 'Action.Submit' || this.action?.type === 'Action.Execute') {
      (this.action as SubmitAction | ExecuteAction).associatedInputs = associatedInputs;
    }
    return this;
  }

  // ─── ExecuteAction specific ───────────────────────────────────────────────

  withVerb(verb: string): this {
    if (this.action?.type === 'Action.Execute') {
      (this.action as ExecuteAction).verb = verb;
    }
    return this;
  }

  // ─── ShowCardAction specific ──────────────────────────────────────────────

  withCard(card: AdaptiveCard): this {
    if (this.action?.type === 'Action.ShowCard') {
      (this.action as ShowCardAction).card = card;
    }
    return this;
  }

  // ─── ToggleVisibilityAction specific ─────────────────────────────────────

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

  build(): AdaptiveAction {
    if (!this.action) {
      throw new Error(
        'No action type specified. Call openUrl(), submit(), showCard(), toggleVisibility(), or execute() first.',
      );
    }
    return this.action;
  }
}
