import { Spacing } from '../../enums.js';
import type { InputNumber } from '../../models.js';

/** Fluent builder for {@link InputNumber} elements. */
export class InputNumberBuilder {
  private readonly input: InputNumber = { type: 'Input.Number', id: '' };

  /** Sets the unique identifier (required for form submission). @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.input.id = id; return this; }
  /** Sets the descriptive label displayed above the input. @param label The label text. @returns The builder instance for method chaining. */
  withLabel(label: string): this { this.input.label = label; return this; }
  /** Sets the placeholder text shown when the input is empty. @param placeholder The placeholder text. @returns The builder instance for method chaining. */
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  /** Sets the initial value. @param value The initial numeric value. @returns The builder instance for method chaining. */
  withValue(value: number): this { this.input.value = value; return this; }
  /** Sets the minimum allowed value. @param min The minimum value. @returns The builder instance for method chaining. */
  withMin(min: number): this { this.input.min = min; return this; }
  /** Sets the maximum allowed value. @param max The maximum value. @returns The builder instance for method chaining. */
  withMax(max: number): this { this.input.max = max; return this; }
  /** Sets whether a value is required. @param isRequired True if required. @returns The builder instance for method chaining. */
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  /** Sets the error message shown when validation fails. @param errorMessage The error message. @returns The builder instance for method chaining. */
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  /** Builds and returns the configured InputNumber element. @returns The configured InputNumber instance. */
  build(): InputNumber { return this.input; }
}
