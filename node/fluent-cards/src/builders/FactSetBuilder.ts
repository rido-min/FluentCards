import { Spacing } from '../enums.js';
import type { FactSet, Fact } from '../models.js';

/** Fluent builder for {@link FactSet} elements. */
export class FactSetBuilder {
  private readonly factSet: FactSet = { type: 'FactSet', facts: [] };

  withId(id: string): this { this.factSet.id = id; return this; }
  withSpacing(spacing: Spacing): this { this.factSet.spacing = spacing; return this; }

  addFact(title: string, value: string): this;
  addFact(fact: Fact): this;
  addFact(titleOrFact: string | Fact, value?: string): this {
    if (typeof titleOrFact === 'string') {
      this.factSet.facts!.push({ title: titleOrFact, value: value! });
    } else {
      this.factSet.facts!.push(titleOrFact);
    }
    return this;
  }

  build(): FactSet { return this.factSet; }
}
