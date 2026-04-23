import { InputLabelPosition, InputStyle, Spacing } from '../../enums.js';
import type { InputTime, AdaptiveElement } from '../../models.js';

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
  /** Sets whether a separator line is displayed above the element. @param separator True to show a separator. @returns The builder instance for method chaining. */
  withSeparator(separator = true): this { this.input.separator = separator; return this; }
  /** Sets whether the element is visible. @param isVisible True to show the element. @returns The builder instance for method chaining. */
  withIsVisible(isVisible: boolean): this { this.input.isVisible = isVisible; return this; }
  /** Sets the height of the element. @param height The height ('auto' or 'stretch'). @returns The builder instance for method chaining. */
  withHeight(height: string): this { this.input.height = height; return this; }
  /** Sets the fallback behavior when the element is unsupported. @param fallback The fallback value ('drop' or an element). @returns The builder instance for method chaining. */
  withFallback(fallback: 'drop' | AdaptiveElement): this { this.input.fallback = fallback; return this; }
  /** Sets the feature requirements for the element. @param key The feature name. @param version The minimum required version. @returns The builder instance for method chaining. */
  withRequires(key: string, version: string): this { this.input.requires = { ...this.input.requires, [key]: version }; return this; }
  /** Sets whether content should be laid out right-to-left. @param rtl True for RTL. @returns The builder instance for method chaining. */
  withRtl(rtl = true): this { this.input.rtl = rtl; return this; }
  /** Sets the label position relative to the input. @param position The label position. @returns The builder instance for method chaining. */
  withLabelPosition(position: InputLabelPosition): this { this.input.labelPosition = position; return this; }
  /** Sets the width of the label. @param width The label width (e.g. '100px' or '30%'). @returns The builder instance for method chaining. */
  withLabelWidth(width: string): this { this.input.labelWidth = width; return this; }
  /** Sets the visual style of the input. @param style The input style. @returns The builder instance for method chaining. */
  withInputStyle(style: InputStyle): this { this.input.inputStyle = style; return this; }

  /** Builds and returns the configured InputTime element. @returns The configured InputTime instance. */
  build(): InputTime { return this.input; }
}
