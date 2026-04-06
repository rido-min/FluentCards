import { Spacing } from '../../enums.js';
import type { InputTime } from '../../models.js';

/** Fluent builder for {@link InputTime} elements. */
export class InputTimeBuilder {
  private readonly input: InputTime = { type: 'Input.Time', id: '' };

  /** Sets the unique identifier (required for form submission). @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.input.id = id; return this; }
  /** Sets the descriptive label displayed above the input. @param label The label text. @returns The builder instance for method chaining. */
  withLabel(label: string): this { this.input.label = label; return this; }
  /** Sets the placeholder text shown when the input is empty. @param placeholder The placeholder text. @returns The builder instance for method chaining. */
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  /** Sets the initial value in `HH:MM` format. @param value The initial time value. @returns The builder instance for method chaining. */
  withValue(value: string): this { this.input.value = value; return this; }
  /** Sets the minimum selectable time in `HH:MM` format. @param min The minimum time. @returns The builder instance for method chaining. */
  withMin(min: string): this { this.input.min = min; return this; }
  /** Sets the maximum selectable time in `HH:MM` format. @param max The maximum time. @returns The builder instance for method chaining. */
  withMax(max: string): this { this.input.max = max; return this; }
  /** Sets whether a value is required. @param isRequired True if required. @returns The builder instance for method chaining. */
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  /** Sets the error message shown when validation fails. @param errorMessage The error message. @returns The builder instance for method chaining. */
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  /** Builds and returns the configured InputTime element. @returns The configured InputTime instance. */
  build(): InputTime { return this.input; }
}
