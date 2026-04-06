import { Spacing } from '../enums.js';
import type { ActionSet } from '../models.js';
import { ActionBuilder } from './ActionBuilder.js';

/** Fluent builder for {@link ActionSet} elements. */
export class ActionSetBuilder {
  private readonly actionSet: ActionSet = { type: 'ActionSet', actions: [] };

  withId(id: string): this { this.actionSet.id = id; return this; }
  withSpacing(spacing: Spacing): this { this.actionSet.spacing = spacing; return this; }

  addAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.actionSet.actions!.push(b.build());
    return this;
  }

  build(): ActionSet { return this.actionSet; }
}
