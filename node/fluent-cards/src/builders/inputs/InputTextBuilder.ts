import type { InputText, AdaptiveAction } from '../../models.js';
import { TextInputStyle, Spacing } from '../../enums.js';
import { ActionBuilder } from '../ActionBuilder.js';

/** Fluent builder for {@link InputText} elements. */
export class InputTextBuilder {
  private readonly input: InputText = { type: 'Input.Text', id: '' };

  /** Sets the unique identifier (required for form submission). @param id The unique identifier. @returns The builder instance for method chaining. */
  withId(id: string): this { this.input.id = id; return this; }
  /** Sets the descriptive label displayed above the input. @param label The label text. @returns The builder instance for method chaining. */
  withLabel(label: string): this { this.input.label = label; return this; }
  /** Sets the placeholder text shown when the input is empty. @param placeholder The placeholder text. @returns The builder instance for method chaining. */
  withPlaceholder(placeholder: string): this { this.input.placeholder = placeholder; return this; }
  /** Sets the initial value. @param value The initial value. @returns The builder instance for method chaining. */
  withValue(value: string): this { this.input.value = value; return this; }
  /** Sets the maximum number of characters allowed. @param maxLength The maximum length. @returns The builder instance for method chaining. */
  withMaxLength(maxLength: number): this { this.input.maxLength = maxLength; return this; }
  /** Sets whether the input allows multiple lines. @param isMultiline True to allow multiple lines. @returns The builder instance for method chaining. */
  withIsMultiline(isMultiline = true): this { this.input.isMultiline = isMultiline; return this; }
  /** Sets the input style (e.g. email, URL, phone). @param style The text input style. @returns The builder instance for method chaining. */
  withStyle(style: TextInputStyle): this { this.input.style = style; return this; }
  /** Sets a regular expression for input validation. @param regex The validation regex. @returns The builder instance for method chaining. */
  withRegex(regex: string): this { this.input.regex = regex; return this; }
  /** Sets whether a value is required. @param isRequired True if required. @returns The builder instance for method chaining. */
  withIsRequired(isRequired = true): this { this.input.isRequired = isRequired; return this; }
  /** Sets the error message shown when validation fails. @param errorMessage The error message. @returns The builder instance for method chaining. */
  withErrorMessage(errorMessage: string): this { this.input.errorMessage = errorMessage; return this; }
  /** Sets the spacing above the element. @param spacing The spacing value. @returns The builder instance for method chaining. */
  withSpacing(spacing: Spacing): this { this.input.spacing = spacing; return this; }

  /** Sets an inline action displayed inside the text input. @param configure A callback to configure the ActionBuilder. @returns The builder instance for method chaining. */
  withInlineAction(configure: (b: ActionBuilder) => void): this {
    const b = new ActionBuilder();
    configure(b);
    this.input.inlineAction = b.build();
    return this;
  }

  /** Builds and returns the configured InputText element. @returns The configured InputText instance. */
  build(): InputText { return this.input; }
}
