import { Spacing } from '../enums.js';
import type { FactSet, Fact, AdaptiveElement } from '../models.js';

/** Fluent builder for {@link FactSet} elements. */
export class FactSetBuilder {
  private readonly factSet: FactSet = { type: 'FactSet', facts: [] };

  /** Sets the unique identifier. @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.factSet.id = id; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.factSet.spacing = spacing; return this; }
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.factSet.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.factSet.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.factSet.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.factSet.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.factSet.requires = { ...this.factSet.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.factSet.rtl = rtl; return this; }

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
