import { Spacing } from '../../enums.js';
import type { InputToggle } from '../../models.js';

/** Fluent builder for {@link InputToggle} elements. */
export class InputToggleBuilder {
  private readonly input: InputToggle = { type: 'Input.Toggle', id: '', title: '' };

  /** Sets the unique identifier (required for form submission). @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.input.id = id; return this; }
  /** Sets the required toggle label displayed beside the checkbox. @param title The toggle label. @returns The builder instance for method chaining. */
  withTitle(title: string): this { this.input.title = title; return this; }
  /** Sets the additional descriptive label displayed above the toggle. @param label The label text. @returns The builder instance for method chaining. */
  withLabel(label: string): this { this.input.label = label; return this; }
  /** Sets the initial value of the toggle. @param value The initial value. @returns The builder instance for method chaining. */
  withValue(value: string): this { this.input.value = value; return this; }
  /** Sets the value submitted when the toggle is on. @param valueOn The on value. @returns The builder instance for method chaining. */
  withValueOn(valueOn: string): this { this.input.valueOn = valueOn; return this; }
  /** Sets the value submitted when the toggle is off. @param valueOff The off value. @returns The builder instance for method chaining. */
  withValueOff(valueOff: string): this { this.input.valueOff = valueOff; return this; }
  /** Sets whether the toggle label wraps. @param wrap True to allow wrapping. @returns The builder instance for method chaining. */
  withWrap(wrap = true): this { this.input.wrap = wrap; return this; }
  /** Sets whether a value is required. @param isRequired True if required. @returns The builder instance for method chaining. */
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  /** Sets the error message shown when validation fails. @param errorMessage The error message. @returns The builder instance for method chaining. */
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  /** Builds and returns the configured InputToggle element. @returns The configured InputToggle instance. */
  build(): InputToggle { return this.input; }
}
