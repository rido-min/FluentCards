import { Spacing } from '../enums.js';
import type { ActionSet, AdaptiveElement } from '../models.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link ActionSet} elements. */
export class ActionSetBuilder {
  private readonly actionSet: ActionSet = { type: 'ActionSet', actions: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.actionSet.id = id; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.actionSet.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.actionSet.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.actionSet.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.actionSet.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.actionSet.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.actionSet.requires = { ...this.actionSet.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.actionSet.rtl = rtl; return this; }

  /** Adds an action to the set. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  addAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.actionSet.actions!.push(b.build());
    return this;
  }

  /** Builds and returns the configured ActionSet. @returns The configured ActionSet instance. */
  build(): ActionSet { return this.actionSet; }
}
