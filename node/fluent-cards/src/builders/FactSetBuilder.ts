import { Spacing } from '../enums.js';
import type { FactSet, Fact } from '../models.js';

/** Fluent builder for {@link FactSet} elements. */
export class FactSetBuilder {
  private readonly factSet: FactSet = { type: 'FactSet', facts: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.factSet.id = id; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.factSet.spacing = spacing; return this; }

  /** Adds a fact with the given title and value. @param title The fact label. @param value The fact value. @returns The builder instance for method chaining. */
  addFact(title: string, value: string): this;
  /** Adds a pre-built Fact object. @param fact A pre-built Fact. @returns The builder instance for method chaining. */
  addFact(fact: Fact): this;
  addFact(titleOrFact: string | Fact, value?: string): this {
    if (typeof titleOrFact === 'string') {
      this.factSet.facts!.push({ title: titleOrFact, value: value! });
    } else {
      this.factSet.facts!.push(titleOrFact);
    }
    return this;
  }

  /** Builds and returns the configured FactSet. @returns The configured FactSet instance. */
  build(): FactSet { return this.factSet; }
}
