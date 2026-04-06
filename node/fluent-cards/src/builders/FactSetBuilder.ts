import { Spacing } from '../enums.js';
import type { FactSet, Fact } from '../models.js';

/** Fluent builder for {@link FactSet} elements. */
export class FactSetBuilder {
  private readonly factSet: FactSet = { type: 'FactSet', facts: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.factSet.id = id; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.factSet.spacing = spacing; return this; }

  /** Adds a fact by title and value, or a pre-built Fact object. @param titleOrFact The title string or a pre-built Fact. @param value The value string (required when title is a string). @returns The builder instance for method chaining. */
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

  /** Builds and returns the configured FactSet. @returns The configured FactSet instance. */
  build(): FactSet { return this.factSet; }
}
