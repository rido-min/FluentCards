import { Spacing } from '../enums.js';
import type { ActionSet } from '../models.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link ActionSet} elements. */
export class ActionSetBuilder {
  private readonly actionSet: ActionSet = { type: 'ActionSet', actions: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.actionSet.id = id; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.actionSet.spacing = spacing; return this; }

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
