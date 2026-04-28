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
import { ActionMode, ActionStyle, AssociatedInputs } from '../enums.js';
import { TeamsDataBuilder } from './TeamsDataBuilder.js';
import { TeamsSubmitPropertiesBuilder } from './TeamsSubmitPropertiesBuilder.js';

/**
 * Fluent builder for {@link AdaptiveAction} instances.
 *
 * Call one of the action-type methods first (`openUrl`, `submit`, etc.)
 * then chain modifiers, then call `build()`.
 */
export class ActionBuilder {
  private action: AdaptiveAction | undefined;
  private dataSet = false;
  private teamsDataSet = false;
  private teamsSubmitTypedSet = false;
  private teamsSubmitRawSet = false;

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

  /** Ensures an action type has been set before modifying properties. @throws Error if no action type has been selected. */
  private ensureActionTypeSet(): void {
    if (!this.action) {
      throw new Error('No action type has been specified. Call openUrl(), submit(), showCard(), toggleVisibility(), or execute() before setting properties.');
    }
  }

  /** Sets the unique identifier for the action. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this {
    this.ensureActionTypeSet();
    this.action!.id = id;
    return this;
  }

  /** Sets the button title. @param title The button label text. @returns The builder instance for method chaining. */
  withTitle(title: string): this {
    this.ensureActionTypeSet();
    this.action!.title = title;
    return this;
  }

  /** Sets the icon URL displayed on the button. @param iconUrl The icon URL. @returns The builder instance for method chaining. */
  withIconUrl(iconUrl: string): this {
    this.ensureActionTypeSet();
    this.action!.iconUrl = iconUrl;
    return this;
  }

  /** Sets the visual style of the action button. @param style The action style. @returns The builder instance for method chaining. */
  withStyle(style: ActionStyle): this {
    this.ensureActionTypeSet();
    this.action!.style = style;
    return this;
  }

  /** Sets whether the action is enabled. @param isEnabled True to enable the action. @returns The builder instance for method chaining. */
  withIsEnabled(isEnabled: boolean): this {
    this.ensureActionTypeSet();
    this.action!.isEnabled = isEnabled;
    return this;
  }

  /** Sets the tooltip shown on hover. @param tooltip The tooltip text. @returns The builder instance for method chaining. */
  withTooltip(tooltip: string): this {
    this.ensureActionTypeSet();
    this.action!.tooltip = tooltip;
    return this;
  }

  /** Sets the action mode (primary or secondary). @param mode The action mode. @returns The builder instance for method chaining. */
  withMode(mode: ActionMode): this {
    this.ensureActionTypeSet();
    this.action!.mode = mode;
    return this;
  }

  /** Sets the feature requirements for the action. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this {
    this.ensureActionTypeSet();
    this.action!.requires = { ...this.action!.requires, [key]: version };
    return this;
  }

  /** Sets the fallback behavior when the action is unsupported. @param fallback The fallback value ('drop' or an action). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveAction): this {
    this.ensureActionTypeSet();
    this.action!.fallback = fallback;
    return this;
  }

  // ─── SubmitAction / ExecuteAction specifics ───────────────────────────────

  /** Sets the data payload (for Submit or Execute actions). @param data The data to submit. @returns The builder instance for method chaining. @throws Error if not a Submit or Execute action, or if WithTeamsData or WithTeamsTaskFetch was already called. */
  withData(data: unknown): this {
    this.ensureActionTypeSet();
    this.ensureSubmitOrExecute('withData');
    if (this.teamsDataSet) {
      throw new Error('Cannot use both withData and withTeamsData on the same action. Use withTeamsData to combine msteams properties with custom data, or withData for raw data.');
    }
    (this.action as SubmitAction | ExecuteAction).data = data;
    this.dataSet = true;
    return this;
  }

  /** Sets which inputs are included when the action is submitted (for Submit or Execute actions). @param associatedInputs The associated inputs option. @returns The builder instance for method chaining. @throws Error if not a Submit or Execute action. */
  withAssociatedInputs(associatedInputs: AssociatedInputs): this {
    this.ensureActionTypeSet();
    this.ensureSubmitOrExecute('withAssociatedInputs');
    (this.action as SubmitAction | ExecuteAction).associatedInputs = associatedInputs;
    return this;
  }

  // ─── ExecuteAction specific ───────────────────────────────────────────────

  /** Sets the verb for an Execute action. @param verb The command verb. @returns The builder instance for method chaining. @throws Error if not an Execute action. */
  withVerb(verb: string): this {
    this.ensureActionTypeSet();
    this.ensureExecuteOnly('withVerb');
    (this.action as ExecuteAction).verb = verb;
    return this;
  }

  // ─── ShowCardAction specific ──────────────────────────────────────────────

  /** Sets the card to reveal for a ShowCard action. @param card The embedded AdaptiveCard. @returns The builder instance for method chaining. */
  withCard(card: AdaptiveCard): this {
    this.ensureActionTypeSet();
    if (this.action!.type === 'Action.ShowCard') {
      (this.action as ShowCardAction).card = card;
    }
    return this;
  }

  // ─── ToggleVisibilityAction specific ─────────────────────────────────────

  /** Adds a target element to toggle for a ToggleVisibility action. @param elementId The ID of the element to toggle. @param isVisible Optional forced visibility state. @returns The builder instance for method chaining. */
  addTargetElement(elementId: string, isVisible?: boolean): this {
    this.ensureActionTypeSet();
    if (this.action!.type === 'Action.ToggleVisibility') {
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

  // ─── Teams-specific methods (Submit-only) ────────────────────────────────

  /** Sets the action data to `{ msteams: { type: 'task/fetch' } }` for Submit actions. @returns The builder instance for method chaining. @throws Error if not a Submit action or if withData was already called. */
  withTeamsTaskFetch(): this {
    this.ensureSubmitOnly('withTeamsTaskFetch');
    this.ensureNoDataConflict();
    const b = new TeamsDataBuilder();
    b.withTaskFetch();
    (this.action as SubmitAction).data = b.build();
    this.teamsDataSet = true;
    return this;
  }

  /** Configures a Teams-specific data payload with msteams properties and/or custom data. Only available on Submit actions. @param configure A callback to configure the TeamsDataBuilder. @returns The builder instance for method chaining. @throws Error if not a Submit action or if withData was already called. */
  withTeamsData(configure: (b: TeamsDataBuilder) => void): this {
    this.ensureSubmitOnly('withTeamsData');
    this.ensureNoDataConflict();
    const b = new TeamsDataBuilder();
    configure(b);
    (this.action as SubmitAction).data = b.build();
    this.teamsDataSet = true;
    return this;
  }

  /** Configures Teams submit feedback properties (e.g. hide feedback). Only available on Submit actions. @param configure A callback to configure the TeamsSubmitPropertiesBuilder. @returns The builder instance for method chaining. @throws Error if not a Submit action or if withTeamsSubmitRaw was already called. */
  withTeamsSubmitFeedback(configure: (b: TeamsSubmitPropertiesBuilder) => void): this {
    this.ensureSubmitOnly('withTeamsSubmitFeedback');
    if (this.teamsSubmitRawSet) {
      throw new Error('Cannot use both withTeamsSubmitFeedback and withTeamsSubmitRaw on the same action. Use one or the other.');
    }
    const b = new TeamsSubmitPropertiesBuilder();
    configure(b);
    (this.action as SubmitAction).msteams = b.build();
    this.teamsSubmitTypedSet = true;
    return this;
  }

  /** Sets the Teams action-level msteams property from a raw object (escape hatch). Only available on Submit actions. @param value The raw msteams properties object. @returns The builder instance for method chaining. @throws Error if not a Submit action or if withTeamsSubmitFeedback was already called. */
  withTeamsSubmitRaw(value: Record<string, unknown>): this {
    this.ensureSubmitOnly('withTeamsSubmitRaw');
    if (this.teamsSubmitTypedSet) {
      throw new Error('Cannot use both withTeamsSubmitFeedback and withTeamsSubmitRaw on the same action. Use one or the other.');
    }
    (this.action as SubmitAction).msteams = { ...value };
    this.teamsSubmitRawSet = true;
    return this;
  }

  private ensureSubmitOrExecute(methodName: string): void {
    if (this.action!.type !== 'Action.Submit' && this.action!.type !== 'Action.Execute') {
      throw new Error(`${methodName}() is only available on Submit or Execute actions. Call submit() or execute() before using this method.`);
    }
  }

  private ensureExecuteOnly(methodName: string): void {
    if (this.action!.type !== 'Action.Execute') {
      throw new Error(`${methodName}() is only available on Execute actions. Call execute() before using this method.`);
    }
  }

  private ensureSubmitOnly(methodName: string): void {
    this.ensureActionTypeSet();
    if (this.action!.type !== 'Action.Submit') {
      throw new Error(`${methodName} is only available on Submit actions. Call submit() before using this method.`);
    }
  }

  private ensureNoDataConflict(): void {
    if (this.dataSet) {
      throw new Error('Cannot use both withData and withTeamsData on the same action. Use withTeamsData to combine msteams properties with custom data, or withData for raw data.');
    }
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
